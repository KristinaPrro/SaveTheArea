using UnityEngine;
using Zenject;

public class LoadingUiInstaller : MonoInstaller
{
	[SerializeField]
	private LoadingUiSettings _uiSettings;
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
	}
}