namespace LiveSplit.Semblance {
	public enum LogObject {
		CurrentSplit,
		GameState,
		Loading,
		StartedGame,
		EndedGame,
		WorldType,
		ActiveScene,
		Infection
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