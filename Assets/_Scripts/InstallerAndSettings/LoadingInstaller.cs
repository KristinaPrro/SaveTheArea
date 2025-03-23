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
	}
}