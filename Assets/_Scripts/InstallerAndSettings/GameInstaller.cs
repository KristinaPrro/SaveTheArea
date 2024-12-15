using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	[SerializeField]
	private GameSettings _settings;
	[SerializeField]
	private List<Transform> _containerEnemySpawn;
	[SerializeField]
	private Transform _containerCharacterSpawn;

	private void OnDestroy()
	{
		_settings = null;
	}

	public override void InstallBindings()
	{
		Container.BindInstances(_settings);

		//Signals
		Container.DeclareSignal<SignalPlayerDamage>();

		//Models
		Container.Bind(typeof(IInputModel), typeof(ITickable))
			.To<DesktopInputModel>()
			.NonLazy();

		Container.BindInterfacesAndSelfTo<EnemySpawnModel>()
			.FromInstance(_containerEnemySpawn)
			.NonLazy();

		//Container.BindInterfacesAndSelfTo<FinishModel>().NonLazy();
		//Container.BindInterfacesAndSelfTo<PlayerModel>().NonLazy();

		//Pools
		//Container.BindMemoryPool<>
	}
}