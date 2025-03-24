using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Zenject;

public class ModelSceneLoader : IInitializable, IDisposable
{
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();
	private readonly int _loadingSceneIndex;
	
	private ReactiveProperty<SceneLoadingState> _loadingState = new(SceneLoadingState.None);
	private ReactiveProperty<float> _progress = new(Utils.FLOAT_DEFAULT_VALUE);
	private int _currentSceneIndex = Utils.INT_DEFAULT_VALUE;
	private float _startTime;
	private float _elapsedTime => Time.realtimeSinceStartup - _startTime;

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

		_loadingState.AsObservable().Subscribe(OnChangeLoadingState).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	private void OnChangeLoadingState(SceneLoadingState state)
	{
		this.LogDebug($"state:{state}; elapsedTime:{_elapsedTime};");
	}

	private void OnChangeScene(SignalCoreChangeScene signalData)
	{
		LoadScene(signalData.SceneType).Forget();
	}

	private async UniTaskVoid LoadScene(SceneType type)
	{
		if (!TryGetSceneIndex(type, out int newSceneIndex))
			return;

		this.LogDebug("Add resource loading and progress animation to the UI!", LogChannel.Todo);
		_progress.Value = 0f;
		_startTime = Time.realtimeSinceStartup;

		//await SetLoadingScene();
		_loadingState.Value = SceneLoadingState.StartLoadingIntermediateScene;
		await SceneManager.LoadSceneAsync(_loadingSceneIndex);
		_progress.Value = 0.15f;

		_loadingState.Value = SceneLoadingState.FinishLoadingIntermediateScene;

		await Clear();
		_progress.Value = 0.3f;

		//todo preload critical assets
		_progress.Value = 0.5f;

		if (!await IsSuccessLoadNewScene(newSceneIndex))
			return;
		
		_loadingState.Value = SceneLoadingState.StartUnloadingOldScene;
		await SceneManager.UnloadSceneAsync(_loadingSceneIndex);
		_loadingState.Value = SceneLoadingState.FinishUnloadingIntermediateScene;

		var newScene = SceneManager.GetSceneByBuildIndex(newSceneIndex);
		SceneManager.SetActiveScene(newScene);

		await UniTask.Delay(SceneUtils.MIN_TIMEOUT_FOR_LOADING_SCENE_MLS);

		_loadingState.Value = SceneLoadingState.Done;

		_progress.Value = Utils.FLOAT_DEFAULT_VALUE;
		_loadingState.Value = SceneLoadingState.None;
	}

	private async UniTask Clear()
	{
		_loadingState.Value = SceneLoadingState.StartClearAssets;
		await UniTask.Yield(PlayerLoopTiming.FixedUpdate);//to do clear assets
		_loadingState.Value = SceneLoadingState.FinishClearAssets;
	}

	private async UniTask<bool> IsSuccessLoadNewScene(int newSceneIndex)
	{
		_loadingState.Value = SceneLoadingState.StartLoadingNewScene;

		var timeoutTime = Time.realtimeSinceStartup + SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE;

		var loadNewSceneOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);
		loadNewSceneOperation.allowSceneActivation = false;

		while (!loadNewSceneOperation.isDone)
		{
			_progress.Value = 0.5f 
				+ Mathf.Clamp01(loadNewSceneOperation.progress / SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED)/2;

			if (Time.realtimeSinceStartup > timeoutTime)
			{
				this.LogError($"Scene loading timed out! Scene with {newSceneIndex} name may be corrupted");
				_loadingState.Value = SceneLoadingState.None;
				return false;
			}

			if (loadNewSceneOperation.progress >= SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED)
			{
				loadNewSceneOperation.allowSceneActivation = true;
			}

			await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
		}
		
		_loadingState.Value = SceneLoadingState.FinishLoadingNewScene;
		return true;
	}

	private bool TryGetSceneIndex(SceneType type, out int newSceneIndex)
	{
		newSceneIndex = Utils.INT_DEFAULT_VALUE;

		if (!SceneUtils.TryGetSceneName(type, out var sceneName))
		{
			this.LogError($"Scene with {type} type is unknown!");
			return false;
		}

		newSceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneName);
		if (newSceneIndex == Utils.INT_DEFAULT_VALUE)
		{
			this.LogError($"Scene with {sceneName} name and {type} type not found!");
			return false;
		}

		return true;
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

	////_loadingState.Value = SceneLoadingState.StartLoadingNewScene;
	////var loadNewSceneOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);
	////loadNewSceneOperation.allowSceneActivation = false;

	////var loadingNewSceneTask = UniTask.WaitUntil(() => loadNewSceneOperation.isDone)
	////	.Timeout(TimeSpan.FromSeconds(SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE));
	////var progressTask = Progress(loadingNewSceneTask);

	////var loadingResult = await UniTask.WhenAny(loadingNewSceneTask, progressTask);

	////if (loadingResult != 0)
	////{
	////	this.LogError($"Scene loading timed out! Scene with {newSceneIndex} name may be corrupted");
	////	_loadingState.Value = SceneLoadingState.None;
	////	return;
	////}

	////_loadingState.Value = SceneLoadingState.FinishLoadingNewScene; 


	////_loadingState.Value = SceneLoadingState.StartLoadingNewScene;
	////var loadNewSceneTask = LoadNewScene(newSceneIndex)
	////	.Timeout(TimeSpan.FromSeconds(SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE))
	////	.SuppressCancellationThrow();
	////var progressTask = Progress();

	////await UniTask.WhenAll(loadNewSceneTask, progressTask);

	////if (!loadNewSceneTask.Status)
	////{
	////	this.LogError($"Scene loading timed out! Scene with {newSceneIndex} name may be corrupted");
	////	_loadingState.Value = SceneLoadingState.None;
	////	return;
	////}

	////_loadingState.Value = SceneLoadingState.FinishLoadingNewScene; 

	////var loadNewSceneOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);

	////var loadingNewSceneTask = UniTask.WaitUntil(() => loadNewSceneOperation.isDone);
	////var timeoutTask = UniTask.Delay(SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE_MLS);

	////var loadingResult = await UniTask.WhenAny(loadingNewSceneTask, timeoutTask);

	////if (loadingResult != 0)
	////{
	////	this.LogError($"Scene loading timed out! Scene with {newSceneIndex} name may be corrupted");
	////	_loadingState.Value = SceneLoadingState.None;
	////	return;
	////}

	////var newScene = SceneManager.GetSceneByBuildIndex(newSceneIndex);
	////SceneManager.SetActiveScene(newScene);
	//}

	//private async UniTask SetLoadingScene()
	//{
	//	var oldScene = SceneManager.GetActiveScene();

	//	_loadingState.Value = SceneLoadingState.StartLoadingIntermediateScene;
	//	await SceneManager.LoadSceneAsync(_loadingSceneIndex, LoadSceneMode.Additive);
	//	var loadingScene = SceneManager.GetSceneByBuildIndex(_loadingSceneIndex);
	//	SceneManager.SetActiveScene(loadingScene);
	//	_loadingState.Value = SceneLoadingState.FinishLoadingIntermediateScene;

	//	if (oldScene != null)
	//	{
	//		_loadingState.Value = SceneLoadingState.StartUnloadingOldScene;
	//		await SceneManager.UnloadSceneAsync(oldScene);
	//		_loadingState.Value = SceneLoadingState.FinishUnloadingOldScene;
	//	}
	//}

}