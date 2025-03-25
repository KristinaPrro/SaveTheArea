public enum SceneLoadingState
{
	None = -1,

	NotStarted = 0,

	StartLoadingIntermediateScene = 1,
	FinishLoadingIntermediateScene = 2,

	StartUnloadingOldScene =5,
	FinishUnloadingOldScene =6,

	StartClearAssets = 10,
	FinishClearAssets = 11,

	StartLoadingNewScene = 15,
	FinishLoadingNewScene = 16,

	Loadassets = 20,

	StartUnloadingIntermediateScene = 25,
	FinishUnloadingIntermediateScene = 26,

	Done = 30,
}