using UnityEngine;
using Zenject;

public class MenuUiInstaller : MonoInstaller
{
	[SerializeField]
	private MenuUiSettings _uiSettings;
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
		//Container.BindInterfacesAndSelfTo<ModelLevelUiInit>().AsSingle().NonLazy();
	}

	private void InstallSignals()
	{
	}

	private void InstallModels()
	{
	}

	private void InstallPools()
	{
	}

	private void InstallPresenters()
	{
	}
}
