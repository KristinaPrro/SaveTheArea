﻿using System;
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
		//Container.DeclareSignal<SignalPlayerDamage>();

		//Models
		Container.Bind(typeof(IInputModel), typeof(ITickable))
			.To<DesktopInputModel>()
			.AsSingle()
			.NonLazy();

		//Container.BindInterfacesAndSelfTo<EnemySpawnModel>()
		//	.FromInstance(_containerEnemySpawn)
		//	.AsSingle()
		//	.NonLazy();

		//Container.BindInterfacesAndSelfTo<FinishModel>().NonLazy();
		//Container.BindInterfacesAndSelfTo<PlayerModel>().NonLazy();

		//Pools
		//Container.BindMemoryPool<>

		//Presenters
		Container.BindViewController<ViewAstronaut, PresenterAstronaut>(
			_settings.ViewAstronaut, _containerCharacterSpawn);
	}
}