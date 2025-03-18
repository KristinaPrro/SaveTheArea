using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ModelSceneLoader : IInitializable, IDisposable
{
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();
	private readonly int _loadingSceneIndex;
	
	private ReactiveProperty<SceneLoadingState> _loadingState = new(SceneLoadingState.None);
	private int _currentSceneIndex = Utils.INT_DEFAULT_VALUE;
	private float _startTime;

	public SceneLoadingState LoadingState => _loadingState.Value;
	public IObservable<SceneLoadingState> LoadingStateStream => _loadingState ;

	public ModelSceneLoader(SignalBus signalBus)
	{
		_signalBus = signalBus;

		_loadingSceneIndex = SceneUtility.GetBuildIndexByScenePath(SceneUtils.LOADING_SCENE_NAME);
	}

	public void Initialize()
	{
		if (_loadingSceneIndex == Utils.INT_DEFAULT_VALUE)
			this.LogError($"Scene with {SceneUtils.LOADING_SCENE_NAME} name not found!");
				
		_signalBus.GetStream<SignalCoreChangeScene>().Subscribe(OnChangeScene).AddTo(_disposables);

		LoadingStateStream.AsObservable().Subscribe(OnChangeLoadingState).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	private void OnChangeScene(SignalCoreChangeScene signalData)
	{
		LoadScene(signalData.SceneType);
	}

	private void OnChangeLoadingState(SceneLoadingState state)
	{
		this.Log($"state:{state}; elapsedTime:{Time.realtimeSinceStartup - _startTime};");
	}

	private void LoadScene(SceneType type, bool isFast = false)
	{
		if (!SceneUtils.TryGetSceneName(type, out var sceneName))
		{
			this.LogError($"Scene with {type} type is unknown!");
			return;
		}

		var newSceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
		if (newSceneIndex == Utils.INT_DEFAULT_VALUE)
		{
			this.LogError($"Scene with {sceneName} name and {type} type not found!");
			return;
		}

		this.LogDebug("Add resource loading and progress animation to the UI!", LogChannel.Todo);
		if (isFast)
			LoadTargetSceneAsyncFast(newSceneIndex).Forget();
		else
			LoadTargetSceneAsync(newSceneIndex).Forget();
	}

	private async UniTaskVoid LoadTargetSceneAsync(int newSceneIndex)
	{
		_startTime = Time.realtimeSinceStartup;
		var currentScene = SceneManager.GetActiveScene();
		var timeoutTask = UniTask.Delay(SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE_MLS); 

		_loadingState.Value = SceneLoadingState.StartLoadingIntermediateScene;
		var loadLoadingSceneOperation = SceneManager.LoadSceneAsync(_loadingSceneIndex, LoadSceneMode.Additive);

		await loadLoadingSceneOperation;
		_loadingState.Value = SceneLoadingState.FinishLoadingIntermediateScene;

		if (currentScene != null)
		{
			_loadingState.Value = SceneLoadingState.StartUnloadingOldScene;			
			var unloadOldSceneOperation = SceneManager.UnloadSceneAsync(currentScene);

			await unloadOldSceneOperation;
			_loadingState.Value = SceneLoadingState.FinishUnloadingOldScene;
		}

		_loadingState.Value = SceneLoadingState.StartClearAssets;
		//to do clear assets
		_loadingState.Value = SceneLoadingState.FinishClearAssets;


		_loadingState.Value = SceneLoadingState.StartLoadingNewScene;
		var loadNewSceneOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);

		var loadingNewSceneTask = UniTask.WaitUntil(() => loadNewSceneOperation.isDone);
		var loadingResult = await UniTask.WhenAny(loadingNewSceneTask, timeoutTask);

		if (loadingResult != 0)
		{
			this.LogError($"Scene loading timed out! Scene with {newSceneIndex} name may be corrupted");
			_loadingState.Value = SceneLoadingState.None;

			return;
		}

		_loadingState.Value = SceneLoadingState.FinishLoadingNewScene;

		await UniTask.Delay(SceneUtils.MIN_TIMEOUT_FOR_LOADING_SCENE_MLS);

		_loadingState.Value = SceneLoadingState.StartUnloadingOldScene;
		var unloadIntermediateSceneOperation = SceneManager.UnloadSceneAsync(_loadingSceneIndex);

		var unloadingIntermediateSceneTask = UniTask.WaitUntil(() => unloadIntermediateSceneOperation.isDone);
		await unloadIntermediateSceneOperation;
		_loadingState.Value = SceneLoadingState.FinishUnloadingIntermediateScene;
		// signal?
		_loadingState.Value = SceneLoadingState.Done;
	}
	
	private async UniTaskVoid LoadTargetSceneAsyncFast(int newSceneIndex)
	{
		_startTime = Time.realtimeSinceStartup;
		var currentScene = SceneManager.GetActiveScene();
		var timeoutTask = UniTask.Delay(SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE_MLS); 

		_loadingState.Value = SceneLoadingState.StartLoadingIntermediateScene;
		var loadLoadingSceneOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);

		await loadLoadingSceneOperation;
		_loadingState.Value = SceneLoadingState.FinishLoadingIntermediateScene;

		if (currentScene != null)
		{
			_loadingState.Value = SceneLoadingState.StartUnloadingOldScene;			
			var unloadOldSceneOperation = SceneManager.UnloadSceneAsync(currentScene);

			await unloadOldSceneOperation;
			_loadingState.Value = SceneLoadingState.FinishUnloadingOldScene;
		}

		_loadingState.Value = SceneLoadingState.Done;
	}
	
	//private async UniTaskVoid LoadSceneAsync(int sceneIndex, float startTime)
	//{
	//	var loadOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

	//	loadOperation.allowSceneActivation = false;

	//	var elapsedTime = 0f;

	//	while (!loadOperation.isDone)
	//	{
	//		var progress = Mathf.Clamp01(loadOperation.progress / SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED);

	//		elapsedTime = Time.realtimeSinceStartup - startTime;
	//		this.LogDebug($"elapsedTime:{elapsedTime};");

	//		if (elapsedTime > SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE)
	//		{
	//			this.LogError($"Scene loading timed out! Scene with {sceneIndex} name may be corrupted");
	//			//todo
	//			return;
	//		}

	//		if (loadOperation.progress >= SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED)
	//		{
	//			loadOperation.allowSceneActivation = true;
	//		}

	//		todo ui
	//		await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
	//	}

	//	load is done
	//	this.Log($"elapsedTime:{elapsedTime};");
	//	if (elapsedTime < SceneUtils.MIN_TIMEOUT_FOR_LOADING_SCENE)
	//	{
	//		var delay = (int)(SceneUtils.MIN_TIMEOUT_FOR_LOADING_SCENE - elapsedTime)
	//			* Utils.TIME_MILLISECONDS_PER_SECOND;

	//		//todo ui

	//		await UniTask.Delay(delay);
	//	}

	//	var newScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
	//	SceneManager.SetActiveScene(newScene);
	//}
}