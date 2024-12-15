using System;
using UnityEngine;
using Zenject;

public class GameUiInstaller : MonoInstaller
{
	[SerializeField]
	private GameUiSettings _uiSettings;
	[SerializeField]
	private GameSettings _settings;
	[SerializeField]
	private Transform _containerScreenPrefabs;
	[SerializeField]
	private Transform _containerDefaultElementPrefabs;

	private void OnDestroy()
	{
		_uiSettings = null;
		_settings = null;
		_containerScreenPrefabs = null;
		_containerDefaultElementPrefabs = null;
	}

	public override void InstallBindings()
	{
		Container.BindInstances(_settings, _uiSettings);

		//Model
		//Container.Bind(typeof(IInitializable), typeof(IDisposable))
		//	.To<SceneUiModel>()
		//	.AsSingle()
		//	.NonLazy();

		//Screens
		//Container.BindViewController<UiViewStatusScreen, UiPresenterStatusScreen>(
		//	_uiSettings.ViewStatusScreen, _containerScreenPrefabs);
		//Container.BindViewController<UiViewResultScreen, UiPresenterResultScreen>(
		//	_uiSettings.ViewResultScreen, _containerScreenPrefabs);

		//Pools
		//Container.BindMemoryPool<>()
	}
}