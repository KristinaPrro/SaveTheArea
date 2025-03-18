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

		InstallSignals();
		InstallModels();
		InstallPools();
		InstallPresenters();
	}

	private void InstallSignals()
	{
	}

	private void InstallModels()
	{
		Container.BindInterfacesAndSelfTo<ModelStartScene>().AsSingle(); 
	}

	private void InstallPools()
	{
	}

	private void InstallPresenters()
	{
	}
}