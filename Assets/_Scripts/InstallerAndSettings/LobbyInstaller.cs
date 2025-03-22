using UnityEngine;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
	[SerializeField]
	private LobbySettings _settings;

	private void OnDestroy()
	{
		_settings = null;
	}

	public override void InstallBindings()
	{
		Container.BindInstances(_settings);

		//InstallSignals
		//InstallModels
		//InstallPools
		//InstallPresenters
	}
}