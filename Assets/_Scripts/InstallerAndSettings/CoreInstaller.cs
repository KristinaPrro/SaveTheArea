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

		//InstallSignals
		Container.DeclareSignal<SignalCoreChangeScene>();

		//InstallModels
		Container.BindInterfacesAndSelfTo<ModelSceneLoader>().AsSingle().NonLazy();
	}
}