using UnityEngine;
using Zenject;

public class LobbyUiInstaller : MonoInstaller
{
	[SerializeField]
	private LobbyUiSettings _uiSettings;
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
		Container.BindInterfacesAndSelfTo<ModelUiScreenChangeInit>().AsSingle().NonLazy();
	}

	private void InstallSignals()
	{
	}

	private void InstallModels()
	{
		Container.BindInterfacesAndSelfTo<ModelUiScreenChange>().AsSingle().NonLazy();
	}

	private void InstallPools()
	{
	}

	private void InstallPresenters()
	{
		Container.BindViewController<UiViewLobbyMain, UiPresenterLobbyMain>(_uiSettings.ViewLobbyMain,
			_containerScreenPrefabs);
	}
}
