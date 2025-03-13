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

	public ModelSceneLoader(SignalBus signalBus)
	{
		_signalBus = signalBus;
	}

	public void Initialize()
	{
		_signalBus.GetStream<SignalCoreChangeScene>().Subscribe(OnChangeScene).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	private void OnChangeScene(SignalCoreChangeScene signalData)
	{
		LoadScene(signalData.SceneType);
	}

	private void LoadScene(SceneType type)
	{
		if (SceneUtils.TryGetSceneName(type, out var sceneName))
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

		LoadSceneAsync(newSceneIndex).Forget();
	}

	private async UniTaskVoid LoadSceneAsync(int sceneIndex)
	{
		var loadOperation = SceneManager.LoadSceneAsync(sceneIndex);

		loadOperation.allowSceneActivation = false;

		var startTime = Time.realtimeSinceStartup;
		var elapsedTime = 0f;

		this.Log($"startTime:{startTime};");

		while (!loadOperation.isDone)
		{
			var progress = Mathf.Clamp01(loadOperation.progress / SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED);

			elapsedTime = Time.realtimeSinceStartup - startTime;
			this.LogDebug($"elapsedTime:{elapsedTime};");

			if (elapsedTime > SceneUtils.MAX_TIMEOUT_FOR_LOADING_SCENE)
			{
				this.LogError($"Scene loading timed out! Scene with {sceneIndex} name may be corrupted");
				//todo
				return;
			}

			if (loadOperation.progress >= SceneUtils.PROGRESS_VALUE_SCENE_ACTIVATED)
			{
				loadOperation.allowSceneActivation = true;
			}

			//todo ui
			await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
		}

		this.Log($"elapsedTime:{elapsedTime};");
		if (elapsedTime < SceneUtils.MIN_TIMEOUT_FOR_LOADING_SCENE)
		{
			var delay = (int)SceneUtils.MIN_TIMEOUT_FOR_LOADING_SCENE * Utils.TIME_MILLISECONDS_PER_SECOND;

			//todo ui

			await UniTask.Delay(delay);
		}

		//todo
		this.Log($"elapsedTime:{Time.realtimeSinceStartup - startTime};");
	}
}