using System;
using UnityEngine;
using Zenject;

public class GameUiInstaller : MonoInstaller
{
	[field: SerializeField]
	private GameUiSettings _uiSettings { get; set; }
	[field: SerializeField]
	private GameSettings _settings { get; set; }
	[field: SerializeField]
	private Transform _containerScreenPrefabs { get; set; }
	[field: SerializeField]
	private Transform _containerDefaultElementPrefabs { get; set; }

	private void OnDestroy()
	{
		_uiSettings = null;
		_settings = null;
		_containerScreenPrefabs = null;
		_containerDefaultElementPrefabs = null;
	}

	public override void InstallBindings()
	{
		//Model
		Container.Bind(typeof(SceneUiModel), typeof(IInitializable), typeof(IDisposable))
			.To<SceneUiModel>()
			.AsSingle()
			.NonLazy(); 
		
		//Screens
		Container.BindViewController<UiViewStatusScreen, UiPresenterStatusScreen>(
			_uiSettings.ViewStatusScreen, _containerScreenPrefabs);
		Container.BindViewController<UiViewResultScreen, UiPresenterResultScreen>(
			_uiSettings.ViewResultScreen, _containerScreenPrefabs);

		//Pools
		//Container.BindMemoryPool<>()
	}
}