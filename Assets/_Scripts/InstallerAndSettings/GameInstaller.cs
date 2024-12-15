using System;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	[field: SerializeField]
	private GameSettings _settings { get; set; }
	[field: SerializeField]
	private ListPool<Transform> _containerEnemySpawn { get; set; }

	private void OnDestroy()
	{
		_settings = null;
	}

	public override void InstallBindings()
	{
		//Models
		Container.Bind(typeof(IInputModel), typeof(IInitializable), typeof(IDisposable))
			.To<DesktopInputModel>()
			.AsSingle()
			.NonLazy();

		Container.BindInterfacesAndSelfTo<EnemySpawnModel>()
			.FromInstance(_containerEnemySpawn)
			.AsSingle()
			.NonLazy();

		Container.BindInterfacesAndSelfTo<FinishModel>().AsSingle().NonLazy();
		Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle().NonLazy();

		//Pools

	}
}