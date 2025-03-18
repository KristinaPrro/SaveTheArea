using UnityEngine;
using Zenject;

public class LoadingInstaller : MonoInstaller
{
	[SerializeField]
	private LoadingSettings _settings;

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
	}

	private void InstallPools()
	{
	}

	private void InstallPresenters()
	{
	}
}