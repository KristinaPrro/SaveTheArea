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

		Container.BindInterfacesAndSelfTo<ModelStartScene>().AsSingle();
	}
}