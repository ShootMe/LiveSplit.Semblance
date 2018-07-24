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
		private bool hasLog = false;
		private Dictionary<LogObject, string> currentValues = new Dictionary<LogObject, string>();
		private Dictionary<string, string> cardStates = new Dictionary<string, string>();
		private bool lastInTransition = false;
		public SplitterComponent(LiveSplitState state) {
			mem = new SplitterMemory();
			foreach (LogObject key in Enum.GetValues(typeof(LogObject))) {
				currentValues[key] = "";
			}

			if (state != null) {
				Model = new TimerModel() { CurrentState = state };
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
				bool inTransition = mem.MouseOverCardInTransition();
				shouldSplit = mem.HasActiveMilestone("chapter1") && inTransition && !lastInTransition;
				lastInTransition = inTransition;
			} else if (Model.CurrentState.CurrentPhase == TimerPhase.Running) {
				bool newChapter = false;
				switch (currentSplit) {
					case 0: newChapter = mem.HasCardState("card002", "bowl=fruit lands"); break;
					case 1: newChapter = mem.HasCardState("card004", "statues=fruit falls"); break;
					case 2: newChapter = mem.HasCardState("card001c", "fruit=final"); break;
					case 3: newChapter = mem.HasCardState("card001c", "char=take fruit"); break;
					case 4: newChapter = mem.HasCardState("card012", "boy_in_tower=take fruit"); break;
					case 5: newChapter = mem.StoryPhase() == StoryPhase.EPILOGUE; break;
				}
				shouldSplit = newChapter && !lastInTransition;
				lastInTransition = newChapter;
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
						case LogObject.GameState: curr = mem.GameState().ToString(); break;
						case LogObject.StoryPhase: curr = mem.StoryPhase().ToString(); break;
						case LogObject.ActiveMilestones: curr = mem.GetActiveMilestones(); break;
						case LogObject.CardStates:
							curr = string.Empty;
							Dictionary<string, string> states = mem.AllCardStates();
							foreach (KeyValuePair<string, string> pair in cardStates) {
								string lastVal = string.Empty;
								if (!states.TryGetValue(pair.Key, out lastVal)) {
									states[pair.Key] = string.Empty;
								}
							}
							foreach (KeyValuePair<string, string> pair in states) {
								string lastVal = string.Empty;
								if (!cardStates.TryGetValue(pair.Key, out lastVal) || lastVal != pair.Value) {
									WriteLogWithTime(pair.Key.ToString() + ": ".PadRight(pair.Key.ToString().Length > 16 ? 0 : 16 - pair.Key.ToString().Length, ' ') + (lastVal ?? string.Empty).PadLeft(25, ' ') + " -> " + pair.Value);
									cardStates[pair.Key] = pair.Value;
								}
							}
							break;
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
			//if (Model.CurrentState.Run.Count == 1 && string.IsNullOrEmpty(Model.CurrentState.Run[0].Name)) {
			//	Model.CurrentState.Run[0].Name = "Red";
			//	Model.CurrentState.Run.AddSegment("Green");
			//	Model.CurrentState.Run.AddSegment("Yellow");
			//	Model.CurrentState.Run.AddSegment("Blue");
			//	Model.CurrentState.Run.AddSegment("Purple");
			//	Model.CurrentState.Run.AddSegment("End");
			//}

			GetValues();
		}
		public void OnReset(object sender, TimerPhase e) {
			currentSplit = -1;
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
		public Control GetSettingsControl(LayoutMode mode) { return null; }
		public void SetSettings(XmlNode document) { }
		public XmlNode GetSettings(XmlDocument document) { return document.CreateElement("Settings"); }
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