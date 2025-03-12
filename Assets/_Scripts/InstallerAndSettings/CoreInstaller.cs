using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller
{
	[SerializeField]
	private CoreSettings _settings;
	[SerializeField]
	private Transform _containerDefaultElementPrefabs;

	private void OnDestroy()
	{
		_settings = null;
	}

	public override void InstallBindings()
	{
		Container.BindInstances(_settings);

		SignalBusInstaller.Install(Container);

		InstallSignals();
		InstallModels();
		InstallPools();
		InstallPresenters();

		//at the end
		//Container.BindInterfacesAndSelfTo<ModelResetLevel>().AsSingle().NonLazy(); 
	}

	private void InstallSignals()
	{
	}

	private void InstallModels()
	{
		Container.BindInterfacesAndSelfTo<ModelSceneLoader>().AsSingle().NonLazy();
	}

	private void InstallPools()
	{
	}

	private void InstallPresenters()
	{
	}
}