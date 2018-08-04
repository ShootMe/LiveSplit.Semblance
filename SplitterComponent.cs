using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
namespace LiveSplit.Semblance {
	public class SplitterComponent : IComponent {
		public TimerModel Model { get; set; }
		public string ComponentName { get { return "Semblance Autosplitter " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3); } }
		public IDictionary<string, Action> ContextMenuControls { get { return null; } }
		private static string LOGFILE = "_Semblance.txt";
		private SplitterMemory mem;
		private int currentSplit = -1, lastLogCheck = 0;
		private bool hasLog = false, lastStarted = false, hasReachedRoom = false, lastShrine = false;
		private string lastScene;
		private SplitterSettings settings;
		private Dictionary<LogObject, string> currentValues = new Dictionary<LogObject, string>();
		public SplitterComponent(LiveSplitState state) {
			mem = new SplitterMemory();
			settings = new SplitterSettings();
			foreach (LogObject key in Enum.GetValues(typeof(LogObject))) {
				currentValues[key] = "";
			}

			if (state != null) {
				Model = new TimerModel() { CurrentState = state };
				Model.InitializeGameTime();
				Model.CurrentState.IsGameTimePaused = true;
				state.OnReset += OnReset;
				state.OnPause += OnPause;
				state.OnResume += OnResume;
				state.OnStart += OnStart;
				state.OnSplit += OnSplit;
				state.OnUndoSplit += OnUndoSplit;
				state.OnSkipSplit += OnSkipSplit;
			}
		}
		private void HandleSplits() {
			bool shouldSplit = false;

			if (currentSplit == -1) {
				bool hasStarted = mem.StartedGame();
				shouldSplit = mem.LastHooked.AddSeconds(5) < DateTime.Now && mem.CurrentGameState() == GameState.NewGame && hasStarted && !lastStarted;
				lastStarted = hasStarted;
			} else if (Model.CurrentState.CurrentPhase == TimerPhase.Running) {
				string scene = mem.ActiveScene();
				bool loading = scene == "EndingPrototype" ? !mem.HasControl() : mem.Loading();
				bool shrine = mem.HasShrine();

				if (currentSplit < Model.CurrentState.Run.Count && currentSplit < settings.Splits.Count) {
					SplitName split = settings.Splits[currentSplit];
					if (scene == "0 - Enforce Collectible") {
						hasReachedRoom = true;
					}

					switch (split) {
						case SplitName.Level_0_1: shouldSplit = scene == "SquishCreationTutorial" && hasReachedRoom && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.World_1: shouldSplit = scene == "World1 - Cuddly" && lastScene == "Overworld Major"; break;
						case SplitName.Level_1_1: shouldSplit = scene == "1 - Intro Deform" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_1_2: shouldSplit = scene == "2 - Deform Puzzles" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_1_2_Shrine: shouldSplit = scene == "2 - Deform Puzzles" && shrine && !lastShrine; break;
						case SplitName.Level_1_3: shouldSplit = scene == "3 - Laser Intro" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_1_4: shouldSplit = scene == "4 - Wall Jumping" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_1_4_Shrine: shouldSplit = scene == "4 - Wall Jumping" && shrine && !lastShrine; break;
						case SplitName.Level_1_5: shouldSplit = scene == "5 - Lasers 2" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_1_5_Shrine: shouldSplit = scene == "5 - Lasers 2" && shrine && !lastShrine; break;
						case SplitName.Level_1_6: shouldSplit = scene == "6 - No Dash Zones" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.World_2: shouldSplit = scene == "World2 - Swamp" && lastScene == "Overworld Major"; break;
						case SplitName.Level_2_1: shouldSplit = scene == "1 - Intro Reset" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_2_2: shouldSplit = scene == "2 - Reset Puzzles" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_2_3: shouldSplit = scene == "3 - Throw up 1" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_2_3_Shrine: shouldSplit = scene == "3 - Throw up 1" && shrine && !lastShrine; break;
						case SplitName.Level_2_4: shouldSplit = scene == "4 - Throw up 2" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_2_4_Shrine: shouldSplit = scene == "4 - Throw up 2" && shrine && !lastShrine; break;
						case SplitName.Level_2_5: shouldSplit = scene == "5 - Throw Side" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_2_6: shouldSplit = scene == "6 - Moving beams" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_2_6_Shrine: shouldSplit = scene == "6 - Moving beams" && shrine && !lastShrine; break;
						case SplitName.World_3: shouldSplit = scene == "World3 - Snow" && lastScene == "Overworld Major"; break;
						case SplitName.Level_3_1: shouldSplit = scene == "1 - Intro hard" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_3_2: shouldSplit = scene == "2 - Intro Puzzle" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_3_2_Shrine: shouldSplit = scene == "2 - Intro Puzzle" && shrine && !lastShrine; break;
						case SplitName.Level_3_3: shouldSplit = scene == "3 - intro beam" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_3_4: shouldSplit = scene == "4 - Reset Beam Complex" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_3_5: shouldSplit = scene == "5 - Reset Beam Throw Up Shape" && mem.InfectionLevel() == 0 && loading && !lastStarted; break;
						case SplitName.Level_3_5_Shrine: shouldSplit = scene == "5 - Reset Beam Throw Up Shape" && shrine && !lastShrine; break;
						case SplitName.World_4: shouldSplit = scene == "EndingPrototype" && lastScene == "Overworld Major"; break;
						case SplitName.Level_4_1: shouldSplit = scene == "EndingPrototype" && mem.XPos() > 42800 && loading && !lastStarted && mem.CurrentGameState() == GameState.Playing && !mem.Dead() && !mem.Loading(); break;
					}
				}

				Model.CurrentState.IsGameTimePaused = Model.CurrentState.CurrentPhase != TimerPhase.Running || (scene == "EndingPrototype" ? mem.Loading() : loading);
				lastStarted = loading;
				lastScene = scene;
				lastShrine = shrine;
			}

			HandleSplit(shouldSplit, false);
		}
		private void HandleSplit(bool shouldSplit, bool shouldReset = false) {
			if (shouldReset) {
				if (currentSplit >= 0) {
					Model.Reset();
				}
			} else if (shouldSplit) {
				if (currentSplit < 0) {
					Model.Start();
				} else {
					Model.Split();
				}
			}
		}
		private void LogValues() {
			if (lastLogCheck == 0) {
				hasLog = File.Exists(LOGFILE);
				lastLogCheck = 300;
			}
			lastLogCheck--;

			if (hasLog || !Console.IsOutputRedirected) {
				string prev = string.Empty, curr = string.Empty;
				foreach (LogObject key in Enum.GetValues(typeof(LogObject))) {
					prev = currentValues[key];

					switch (key) {
						case LogObject.CurrentSplit: curr = currentSplit.ToString(); break;
						case LogObject.GameState: curr = mem.CurrentGameState().ToString(); break;
						case LogObject.Loading: curr = mem.Loading().ToString(); break;
						case LogObject.StartedGame: curr = mem.StartedGame().ToString(); break;
						case LogObject.EndedGame: curr = mem.EndedGame().ToString(); break;
						case LogObject.WorldType: curr = mem.ActiveWorld().ToString(); break;
						case LogObject.ActiveScene: curr = mem.ActiveScene(); break;
						case LogObject.Infection: curr = mem.InfectionLevel().ToString(); break;
						case LogObject.Dead: curr = mem.Dead().ToString(); break;
						case LogObject.HasControl: curr = mem.HasControl().ToString(); break;
						case LogObject.XPos: curr = mem.XPos().ToString("0"); break;
						case LogObject.Checkpoint: curr = mem.Checkpoint().ToString(); break;
						case LogObject.Shrines: curr = mem.HasShrine().ToString(); break;
						default: curr = string.Empty; break;
					}

					if (prev == null) { prev = string.Empty; }
					if (curr == null) { curr = string.Empty; }
					if (!prev.Equals(curr)) {
						WriteLogWithTime(key.ToString() + ": ".PadRight(16 - key.ToString().Length, ' ') + prev.PadLeft(25, ' ') + " -> " + curr);

						currentValues[key] = curr;
					}
				}
			}
		}
		private void WriteLog(string data) {
			if (hasLog || !Console.IsOutputRedirected) {
				if (!Console.IsOutputRedirected) {
					Console.WriteLine(data);
				}
				if (hasLog) {
					using (StreamWriter wr = new StreamWriter(LOGFILE, true)) {
						wr.WriteLine(data);
					}
				}
			}
		}
		private void WriteLogWithTime(string data) {
			WriteLog(DateTime.Now.ToString(@"HH\:mm\:ss.fff") + (Model != null && Model.CurrentState.CurrentTime.RealTime.HasValue ? " | " + Model.CurrentState.CurrentTime.RealTime.Value.ToString("G").Substring(3, 11) : "") + ": " + data);
		}
		public void GetValues() {
			if (!mem.HookProcess()) { return; }

			if (Model != null) {
				HandleSplits();
			}

			LogValues();
		}
		public void Update(IInvalidator invalidator, LiveSplitState lvstate, float width, float height, LayoutMode mode) {
			GetValues();
		}
		public void OnReset(object sender, TimerPhase e) {
			currentSplit = -1;
			hasReachedRoom = false;
			lastStarted = true;
			Model.CurrentState.IsGameTimePaused = true;
			WriteLog("---------Reset----------------------------------");
		}
		public void OnResume(object sender, EventArgs e) {
			WriteLog("---------Resumed--------------------------------");
		}
		public void OnPause(object sender, EventArgs e) {
			WriteLog("---------Paused---------------------------------");
		}
		public void OnStart(object sender, EventArgs e) {
			currentSplit = 0;
			hasReachedRoom = false;
			lastStarted = true;
			WriteLog("---------New Game " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3) + "-------------------------");
		}
		public void OnUndoSplit(object sender, EventArgs e) {
			currentSplit--;
			WriteLog("---------Undo-----------------------------------");
		}
		public void OnSkipSplit(object sender, EventArgs e) {
			currentSplit++;
			WriteLog("---------Skip-----------------------------------");
		}
		public void OnSplit(object sender, EventArgs e) {
			currentSplit++;
			WriteLog("---------Split----------------------------------");
		}
		public Control GetSettingsControl(LayoutMode mode) { return settings; }
		public void SetSettings(XmlNode document) { settings.SetSettings(document); }
		public XmlNode GetSettings(XmlDocument document) { return settings.UpdateSettings(document); }
		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) { }
		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) { }
		public float HorizontalWidth { get { return 0; } }
		public float MinimumHeight { get { return 0; } }
		public float MinimumWidth { get { return 0; } }
		public float PaddingBottom { get { return 0; } }
		public float PaddingLeft { get { return 0; } }
		public float PaddingRight { get { return 0; } }
		public float PaddingTop { get { return 0; } }
		public float VerticalHeight { get { return 0; } }
		public void Dispose() { }
	}
}