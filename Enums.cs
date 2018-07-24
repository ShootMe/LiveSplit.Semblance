namespace LiveSplit.Semblance {
	public enum LogObject {
		CurrentSplit,
		GameState,
		StoryPhase,
		ActiveMilestones,
		CardStates
	}
	public enum GameState {
		SHOW_SPLASH,
		SHOWING_SPLASH,
		LOADING,
		LOADED,
		FADEIN_GAME,
		PLAY_GAME,
		FADEOUT_GAME,
		HOLD_AFTER_LOAD,
		SHOW_END_GAME,
		SHOWING_END_GAME
	}
	public enum StoryPhase {
		PROLOGUE,
		CHAPTERS,
		EPILOGUE,
		ENDING
	}
}