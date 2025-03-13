public static class SceneUtils
{
	public const float PROGRESS_VALUE_SCENE_ACTIVATED = 0.9f;
	public const float MAX_TIMEOUT_FOR_LOADING_SCENE = 10f;
	public const float MIN_TIMEOUT_FOR_LOADING_SCENE = 1f;

	public const string LOBBY_SCENE_NAME = "LobbyScene";
	public const string LOADING_SCENE_NAME = "LoadingScene";
	public const string SAVE_AREA_SCENE_NAME = "GameScene";

	public static bool TryGetSceneName(SceneType type, out string sceneName)
	{
		sceneName = "";
		switch (type)
		{
			case SceneType.Lobby:
				sceneName = LOBBY_SCENE_NAME;
				break;
			case SceneType.Loading:
				sceneName = LOADING_SCENE_NAME;
				break;
			case SceneType.SaveArea:
				sceneName = SAVE_AREA_SCENE_NAME;
				break;
			default:
				return false;
		}

		return true;
	}
}