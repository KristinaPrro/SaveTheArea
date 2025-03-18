public static class SceneUtils
{
	public const float PROGRESS_VALUE_SCENE_ACTIVATED = 0.9f;
	public const float MAX_TIMEOUT_FOR_LOADING_SCENE = 10f;
	public const float MIN_TIMEOUT_FOR_LOADING_SCENE = 1f;

	public const int MAX_TIMEOUT_FOR_LOADING_SCENE_MLS = 
		(int)MAX_TIMEOUT_FOR_LOADING_SCENE * Utils.TIME_MILLISECONDS_PER_SECOND;
	public const int MIN_TIMEOUT_FOR_LOADING_SCENE_MLS = 
		(int)MIN_TIMEOUT_FOR_LOADING_SCENE * Utils.TIME_MILLISECONDS_PER_SECOND;

	public const string START_SCENE_NAME = "StartScene";
	public const string LOADING_SCENE_NAME = "LoadingScene";
	public const string LOBBY_SCENE_NAME = "LobbyScene";
	public const string SAVE_AREA_SCENE_NAME = "GameScene";

	public static bool TryGetSceneName(SceneType type, out string sceneName)
	{
		sceneName = "";
		switch (type)
		{
			case SceneType.Start:
				sceneName = START_SCENE_NAME;
				break;

			case SceneType.Lobby:
				sceneName = LOBBY_SCENE_NAME;
				break;

			case SceneType.SaveArea:
				sceneName = SAVE_AREA_SCENE_NAME;
				break;

			case SceneType.Loading: //only intermediate
			default:
				return false;
		}

		return true;
	}
}