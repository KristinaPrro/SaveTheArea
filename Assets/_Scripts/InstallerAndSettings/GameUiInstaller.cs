using UnityEngine;
using Zenject;

public class GameUiInstaller : MonoInstaller
{
	[SerializeField]
	private GameUiSettings _uiSettings;
	[SerializeField]
	private Transform _containerScreenPrefabs;


	private void OnDestroy()
	{
		_uiSettings = null;
		_containerScreenPrefabs = null;
	}

	public override void InstallBindings()
	{
		Container.BindInstances(_uiSettings);

		InstallSignals();
		InstallModels();
		InstallPools();
		InstallPresenters();
		
		//at the end
		Container.BindInterfacesAndSelfTo<ModelLevelUiInit>().AsSingle().NonLazy();
	}

	private void InstallSignals()
	{
	}

	private void InstallModels()
	{
		Container.BindInterfacesAndSelfTo<ModelLevelUi>().AsSingle().NonLazy();
		Container.BindInterfacesAndSelfTo<ModelLevelUiAutoWindowChange>().AsSingle().NonLazy();
	}

	private void InstallPools()
	{
	}

	private void InstallPresenters()
	{
		Container.BindViewController<UiViewGameStatusScreen, UiPresenterGameStatusScreen>(_uiSettings.ViewStatusScreen, _containerScreenPrefabs);
		Container.BindViewController<UiViewResultScreen, UiPresenterResultScreen>(_uiSettings.ViewResultScreen, _containerScreenPrefabs);
	}
}