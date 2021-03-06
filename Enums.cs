﻿using System.ComponentModel;

namespace LiveSplit.Semblance {
	public enum LogObject {
		CurrentSplit,
		GameState,
		Loading,
		StartedGame,
		EndedGame,
		WorldType,
		ActiveScene,
		Infection,
		Dead,
		HasControl,
		XPos,
		Checkpoint,
		Shrines
	}
	public enum SplitName {
		[Description("Manual Split (Not Automatic)"), ToolTip("Does not split automatically. Use this for custom splits not yet defined.")]
		ManualSplit,

		[Description("Intro (Completed)"), ToolTip("Splits when completing the intro")]
		Level_0_1,

		[Description("World 1 (Entered)"), ToolTip("Splits when entering world 1")]
		World_1,
		[Description("1 - 1 (Completed)"), ToolTip("Splits when completing level 1 - 1")]
		Level_1_1,
		[Description("1 - 2 (Completed)"), ToolTip("Splits when completing level 1 - 2")]
		Level_1_2,
		[Description("1 - 2 (Shrine)"), ToolTip("Splits when finding the shrine in level 1 - 2")]
		Level_1_2_Shrine,
		[Description("1 - 3 (Completed)"), ToolTip("Splits when completing level 1 - 3")]
		Level_1_3,
		[Description("1 - 4 (Completed)"), ToolTip("Splits when completing level 1 - 4")]
		Level_1_4,
		[Description("1 - 4 (Shrine)"), ToolTip("Splits when finding the shrine in level 1 - 4")]
		Level_1_4_Shrine,
		[Description("1 - 5 (Completed)"), ToolTip("Splits when completing level 1 - 5")]
		Level_1_5,
		[Description("1 - 5 (Shrine)"), ToolTip("Splits when finding the shrine in level 1 - 5")]
		Level_1_5_Shrine,
		[Description("1 - 6 (Completed)"), ToolTip("Splits when completing level 1 - 6")]
		Level_1_6,

		[Description("World 2 (Entered)"), ToolTip("Splits when entering world 2")]
		World_2,
		[Description("2 - 1 (Completed)"), ToolTip("Splits when completing level 2 - 1")]
		Level_2_1,
		[Description("2 - 2 (Completed)"), ToolTip("Splits when completing level 2 - 2")]
		Level_2_2,
		[Description("2 - 3 (Completed)"), ToolTip("Splits when completing level 2 - 3")]
		Level_2_3,
		[Description("2 - 3 (Shrine)"), ToolTip("Splits when finding the shrine in level 2 - 3")]
		Level_2_3_Shrine,
		[Description("2 - 4 (Completed)"), ToolTip("Splits when completing level 2 - 4")]
		Level_2_4,
		[Description("2 - 4 (Shrine)"), ToolTip("Splits when finding the shrine in level 2 - 4")]
		Level_2_4_Shrine,
		[Description("2 - 5 (Completed)"), ToolTip("Splits when completing level 2 - 5")]
		Level_2_5,
		[Description("2 - 6 (Completed)"), ToolTip("Splits when completing level 2 - 6")]
		Level_2_6,
		[Description("2 - 6 (Shrine)"), ToolTip("Splits when finding the shrine in level 2 - 6")]
		Level_2_6_Shrine,

		[Description("World 3 (Entered)"), ToolTip("Splits when entering world 3")]
		World_3,
		[Description("3 - 1 (Completed)"), ToolTip("Splits when completing level 3 - 1")]
		Level_3_1,
		[Description("3 - 2 (Completed)"), ToolTip("Splits when completing level 3 - 2")]
		Level_3_2,
		[Description("3 - 2 (Shrine)"), ToolTip("Splits when finding the shrine in level 3 - 2")]
		Level_3_2_Shrine,
		[Description("3 - 3 (Completed)"), ToolTip("Splits when completing level 3 - 3")]
		Level_3_3,
		[Description("3 - 4 (Completed)"), ToolTip("Splits when completing level 3 - 4")]
		Level_3_4,
		[Description("3 - 5 (Completed)"), ToolTip("Splits when completing level 3 - 5")]
		Level_3_5,
		[Description("3 - 5 (Shrine)"), ToolTip("Splits when finding the shrine in level 3 - 5")]
		Level_3_5_Shrine,

		[Description("World 4 (Entered)"), ToolTip("Splits when entering world 4")]
		World_4,
		[Description("4 - 1 (Completed)"), ToolTip("Splits when completing level 4 - 1")]
		Level_4_1,
	}
	public enum GameState {
		NewGame,
		ExistingGame,
		Playing,
		Paused,
		Logos
	}
	public enum WorldType {
		Cuddly,
		Swamp,
		Snow,
		Crystal
	}
	public enum PortalType {
		UNKNOWN,
		BEGINNING,
		END
	}
}