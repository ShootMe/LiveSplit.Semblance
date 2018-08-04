using System;
using System.Diagnostics;
namespace LiveSplit.Semblance {
	public partial class SplitterMemory {
		private static ProgramPointer GameManager = new ProgramPointer(AutoDeref.Single, new ProgramSignature(PointerVersion.Steam, "558BEC5783EC548B7D0883EC0C57E8????????83C410BA", 23));
		private static ProgramPointer LevelManager = new ProgramPointer(AutoDeref.Single, new ProgramSignature(PointerVersion.Steam, "558BEC5783EC048B7D08B8????????8938BA????????E8", 11));
		private static ProgramPointer CharacterBehaviour = new ProgramPointer(AutoDeref.Single, new ProgramSignature(PointerVersion.Steam, "89388B473483EC086A00503900E8????????83C410", -4));
		public Process Program { get; set; }
		public bool IsHooked { get; set; } = false;
		public DateTime LastHooked;

		public SplitterMemory() {
			LastHooked = DateTime.MinValue;
		}

		public GameState CurrentGameState() {
			//GameManager.instance._stateMachine.currentState
			return (GameState)GameManager.Read<int>(Program, 0xc, 0x0, 0x34, 0x10);
		}
		public WorldType ActiveWorld() {
			//GameManager.instance.SavedGameData.LastActiveWorld
			return (WorldType)GameManager.Read<int>(Program, 0xc, 0x0, 0x10, 0x24);
		}
		public bool Loading() {
			//GameManager.instance.LoadingScene
			return GameManager.Read<bool>(Program, 0xc, 0x0, 0x74);
		}
		public bool StartedGame() {
			//GameManager.instance._hasStartedGame
			return GameManager.Read<bool>(Program, 0xc, 0x0, 0x76);
		}
		public bool EndedGame() {
			//GameManager.instance.SavedGameData.HasFinishedEnding
			return GameManager.Read<bool>(Program, 0xc, 0x0, 0x10, 0x2c);
		}
		public string ActiveScene() {
			//GameManager.instance.SavedGameData.LastScene
			return GameManager.Read(Program, 0xc, 0x0, 0x10, 0x14, 0x0);
		}
		public bool Dead() {
			//CharacterBehaviour.instance._isDead
			return CharacterBehaviour.Read<bool>(Program, 0x0, 0xdb);
		}
		public bool HasControl() {
			//CharacterBehaviour.instance.HasControl
			return CharacterBehaviour.Read<bool>(Program, 0x0, 0xa7);
		}
		public float XPos() {
			//CharacterBehaviour.instance._dashStartPos
			return CharacterBehaviour.Read<float>(Program, 0x0, 0xcc);
		}
		public int Checkpoint() {
			string scene = ActiveScene();
			if (string.IsNullOrEmpty(scene)) { return -1; }

			IntPtr checkpoints = (IntPtr)LevelManager.Read<uint>(Program, 0x0, 0x30);
			int size = Program.Read<int>(checkpoints, 0xc);
			for (int i = 0; i < size; i++) {
				bool isActive = Program.Read<bool>(checkpoints, 0x10 + (i * 4), 0x1c);
				if (isActive) {
					return i;
				}
			}
			return -1;
		}
		public bool HasShrine() {
			string scene = ActiveScene();
			if (string.IsNullOrEmpty(scene)) { return false; }

			IntPtr shrines = (IntPtr)LevelManager.Read<uint>(Program, 0x0, 0x1c);
			if (shrines == IntPtr.Zero) { return false; }

			int size = Program.Read<int>(shrines, 0xc);
			if (size == 0) { return false; }

			for (int i = 0; i < size; i++) {
				if (Program.Read<bool>(shrines, 0x10 + (i * 4), 0x18, 0xc)) {
					return true;
				}
			}
			return false;
		}
		public float InfectionLevel() {
			string scene = ActiveScene();
			if (string.IsNullOrEmpty(scene)) { return 1f; }

			IntPtr collects = (IntPtr)LevelManager.Read<uint>(Program, 0x0, 0x28, 0xc);
			if (collects == IntPtr.Zero) { return 1f; }

			int size = Program.Read<int>(collects, 0xc);
			if (size == 0) { return 1f; }

			int total = 0;
			for (int i = 0; i < size; i++) {
				if (Program.Read<bool>(collects, 0x10 + (i * 4), 0xc)) {
					total++;
				}
			}
			return 1f - ((float)total / size);
		}
		public bool HookProcess() {
			IsHooked = Program != null && !Program.HasExited;
			if (!IsHooked && DateTime.Now > LastHooked.AddSeconds(1)) {
				LastHooked = DateTime.Now;
				Process[] processes = Process.GetProcessesByName("Semblance");
				Program = processes != null && processes.Length > 0 ? processes[0] : null;

				if (Program != null && !Program.HasExited) {
					MemoryReader.Update64Bit(Program);
					IsHooked = true;
				}
			}

			return IsHooked;
		}
		public void Dispose() {
			if (Program != null) {
				Program.Dispose();
			}
		}
	}
}