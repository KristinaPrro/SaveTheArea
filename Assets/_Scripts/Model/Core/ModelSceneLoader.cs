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
		LoadSceneAsync(signalData.SceneType).Forget();
	}

	private async UniTaskVoid LoadSceneAsync(SceneType type)
	{
		if (!TryGetSceneIndex(type, out int newSceneIndex))
			return;

		this.LogDebug("Add resource loading and progress animation to the UI!", LogChannel.Todo);
		_progress.Value = 0f;
		_startTime = Time.realtimeSinceStartup;

		//await SetLoadingScene();
		_loadingState.Value = SceneLoadingState.StartLoadingIntermediateScene;
		await SceneManager.LoadSceneAsync(_loadingSceneIndex);
		_loadingState.Value = SceneLoadingState.FinishLoadingIntermediateScene;

		await ClearAsync();
		_progress.Value = SceneUtils.PROGRESS_VALUE_START_LOADING_NEW_SCENE;

		if (!await TryLoadNewSceneAsync(newSceneIndex))
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

	private async UniTask ClearAsync()
	{
		_loadingState.Value = SceneLoadingState.StartClearAssets;
		
		var unloadResourcesOperation = Resources.UnloadUnusedAssets();
		//var unloadAddressablesOperation = Addressables.Release(); //to do

		while (!unloadResourcesOperation.isDone)
		{
			_progress.Value = SceneUtils.PROGRESS_VALUE_START_LOADING_NEW_SCENE 
				* Mathf.Clamp01(unloadResourcesOperation.progress);

			await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
		}

		_loadingState.Value = SceneLoadingState.FinishClearAssets;
	}

	private async UniTask<bool> TryLoadNewSceneAsync(int newSceneIndex)
	{
		_loadingState.Value = SceneLoadingState.StartLoadingNewScene;

		var timeoutTime = Time.realtimeSinceStartup + SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE;

		var loadNewSceneOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);
		loadNewSceneOperation.allowSceneActivation = false;

		while (!loadNewSceneOperation.isDone)
		{
			_progress.Value = SceneUtils.PROGRESS_VALUE_START_LOADING_NEW_SCENE 
				+ SceneUtils.PROGRESS_VALUE_START_LOADING_NEW_SCENE_REVERS
				* Mathf.Clamp01(loadNewSceneOperation.progress / SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED);

			if (Time.realtimeSinceStartup > timeoutTime)
			{
				this.LogError($"Scene loading timed out! Scene with {newSceneIndex} name may be corrupted");
				_loadingState.Value = SceneLoadingState.None;
				return false;
			}

			if (loadNewSceneOperation.progress >= SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED)
			{
				//todo preload critical assets

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
}