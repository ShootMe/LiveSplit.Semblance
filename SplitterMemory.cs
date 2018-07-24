using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace LiveSplit.Semblance {
	public partial class SplitterMemory {
		private static ProgramPointer Startup = new ProgramPointer(AutoDeref.Single, new ProgramSignature(PointerVersion.Steam, "558BEC535783EC308B7D088B05", 13));
		public Process Program { get; set; }
		public bool IsHooked { get; set; } = false;
		private DateTime LastHooked;

		public SplitterMemory() {
			LastHooked = DateTime.MinValue;
		}

		public GameState GameState() {
			//Startup.s_startup.m_gameState
			return (GameState)Startup.Read<int>(Program, 0x0, 0x180);
		}
		public float GameTime() {
			//Startup.s_startup.m_gameTime
			return Startup.Read<float>(Program, 0x0, 0x188);
		}
		public StoryPhase StoryPhase() {
			//Startup.s_startup.m_panel.currentPage
			return (StoryPhase)Startup.Read<int>(Program, 0x0, 0x44, 0xe8);
		}
		public string MouseOverCard() {
			//Startup.s_startup.m_panel.m_mouseOverCard.m_card.m_id
			return Startup.Read(Program, 0x0, 0x44, 0x94, 0xc, 0xc, 0x0);
		}
		public bool MouseOverCardInTransition() {
			//Startup.s_startup.m_panel.m_mouseOverCard.m_inTransition
			return Startup.Read<bool>(Program, 0x0, 0x44, 0x94, 0x75);
		}
		public bool HasCardState(string card, string state) {
			//Startup.s_startup.m_panel.m_cardViewers
			IntPtr viewers = (IntPtr)Startup.Read<uint>(Program, 0x0, 0x44, 0x70);
			int size = Program.Read<int>(viewers, 0x20);
			viewers = (IntPtr)Program.Read<uint>(viewers, 0x14);

			for (int i = 0; i < size; i++) {
				IntPtr viewer = (IntPtr)Program.Read<uint>(viewers, 0x10 + (i * 4));
				string currentCard = Program.ReadString(viewer, 0xc, 0xc, 0x0);
				if (currentCard.Equals(card, StringComparison.OrdinalIgnoreCase)) {
					return HasCardState(viewer, state);
				}
			}
			return false;
		}
		private bool HasCardState(IntPtr cardViewer, string state) {
			//cardViewer.m_states
			IntPtr states = (IntPtr)Program.Read<uint>(cardViewer, 0x50);
			int size = Program.Read<int>(states, 0x20);
			IntPtr keys = (IntPtr)Program.Read<uint>(states, 0x10);
			states = (IntPtr)Program.Read<uint>(states, 0x14);

			for (int i = 0; i < size; i++) {
				string key = Program.ReadString(keys, 0x10 + (i * 4), 0xc, 0x0);
				string value = Program.ReadString(states, 0x10 + (i * 4), 0xc, 0x0);
				if ((key + "=" + value).Equals(state, StringComparison.OrdinalIgnoreCase)) {
					return true;
				}
			}
			return false;
		}
		public Dictionary<string, string> AllCardStates() {
			//Startup.s_startup.m_panel.m_cardViewers
			IntPtr viewers = (IntPtr)Startup.Read<uint>(Program, 0x0, 0x44, 0x70);
			int size = Program.Read<int>(viewers, 0x20);
			viewers = (IntPtr)Program.Read<uint>(viewers, 0x14);
			Dictionary<string, string> states = new Dictionary<string, string>();
			for (int i = 0; i < size; i++) {
				IntPtr viewer = (IntPtr)Program.Read<uint>(viewers, 0x10 + (i * 4));
				CardStates(viewer, states);
			}

			return states;
		}
		private void CardStates(IntPtr cardViewer, Dictionary<string, string> cardStates) {
			//cardViewer.m_states
			IntPtr states = (IntPtr)Program.Read<uint>(cardViewer, 0x50);

			int size = Program.Read<int>(states, 0x20);
			IntPtr keys = (IntPtr)Program.Read<uint>(states, 0x10);
			states = (IntPtr)Program.Read<uint>(states, 0x14);
			string card = Program.ReadString(cardViewer, 0xc, 0xc, 0x0);

			for (int i = 0; i < size; i++) {
				string key = Program.ReadString(keys, 0x10 + (i * 4), 0xc, 0x0);
				string value = Program.ReadString(states, 0x10 + (i * 4), 0xc, 0x0);
				cardStates[card + "_" + key] = value;
			}
		}
		public string GetActiveMilestones() {
			//Startup.s_startup.m_panel.m_milestoneManger.m_activeMilestoneBySequence
			IntPtr milestones = (IntPtr)Startup.Read<uint>(Program, 0x0, 0x44, 0x50, 0x18);

			int listSize = Program.Read<int>(milestones, 0x20);
			milestones = (IntPtr)Program.Read<uint>(milestones, 0x14);
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < listSize; i++) {
				string milestone = Program.ReadString(milestones, 0x10 + (i * 4), 0xc, 0x0);
				if (!string.IsNullOrEmpty(milestone) && milestone.IndexOf("hover m") < 0) {
					sb.Append(milestone).Append(" | ");
				}
			}
			if (sb.Length > 0) { sb.Length -= 3; }

			return sb.ToString();
		}
		public bool HasActiveMilestone(string milestone) {
			//Startup.s_startup.m_panel.m_milestoneManger.m_activeMilestoneBySequence
			IntPtr milestones = (IntPtr)Startup.Read<uint>(Program, 0x0, 0x44, 0x50, 0x18);

			int listSize = Program.Read<int>(milestones, 0x20);
			milestones = (IntPtr)Program.Read<uint>(milestones, 0x14);
			for (int i = 0; i < listSize; i++) {
				string current = Program.ReadString(milestones, 0x10 + (i * 4), 0xc, 0x0);
				if (current.Equals(milestone, StringComparison.OrdinalIgnoreCase)) {
					return true;
				}
			}
			return false;
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