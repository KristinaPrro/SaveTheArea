using UnityEngine;
using Zenject;

public class StartInstaller : MonoInstaller
{
	[SerializeField]
	private StartSettings _settings;

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
	}

	private void InstallPools()
	{
	}

	private void InstallPresenters()
	{
	}
}