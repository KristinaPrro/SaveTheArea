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
	[SerializeField]
	private Transform _containerBulletSpawn;
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

		InstallSignals();
		InstallModels();
		InstallPools();
		InstallPresenters();
	}

	private void InstallSignals()
	{
		Container.DeclareSignal<SignalPlayerDamage>();
		Container.DeclareSignal<SignalEnemyDamage>();
		Container.DeclareSignal<SignalEnemyReachedFinish>();
		Container.DeclareSignal<SignalPlayerFire>();
		Container.DeclareSignal<SignalDisappearanceDamageElement>();
	}

	private void InstallModels()
	{
		Container.Bind(typeof(IInputModel), typeof(ITickable))
			.To<ModelInputDesktop>()
			.AsSingle()
			.NonLazy();

		Container.BindInterfacesAndSelfTo<ModelEnemy>().AsSingle();
		Container.BindInterfacesAndSelfTo<ModelEnemyObjects>().AsSingle();
		Container.BindInterfacesAndSelfTo<ModelSpawnEnemy>()
			.AsSingle()
			.WithArguments(_containerEnemySpawn)
			.NonLazy();

		Container.BindInterfacesAndSelfTo<ModelPlayerDamageElementBullets>().AsSingle().NonLazy();
		Container.BindInterfacesAndSelfTo<ModelPlayerDamageElements>().AsSingle();
		Container.BindInterfacesAndSelfTo<ModelPlayerSpawnDamageElement>()
			.AsSingle().WithArguments(_containerBulletSpawn);
		Container.BindInterfacesAndSelfTo<ModelPlayerTargetEnemys>().AsSingle().NonLazy();
		Container.BindInterfacesAndSelfTo<ModelPlayerAttack>().AsSingle().NonLazy();

		//Container.BindInterfacesAndSelfTo<FinishModel>().NonLazy();
		//Container.BindInterfacesAndSelfTo<PlayerModel>().NonLazy();
	}

	private void InstallPools()
	{
		Container.BindMemoryPool<ViewPoolEnemyRobotGray, ViewPoolEnemyRobotGray.Pool>()
			.WithInitialSize(_settings.GetPoolItem(PoolItemType.RobotGray).Count)
			.FromComponentInNewPrefab(_settings.GetPoolItem(PoolItemType.RobotGray).ItemGameObject)
			.UnderTransform(_containerDefaultElementPrefabs);

		Container.BindFactory<Transform, PresenterPoolEnemyRobotGray, PresenterPoolEnemyRobotGray.Factory>()
			.FromFactory<PooledViewPresenterFactory<PresenterPoolEnemyRobotGray, ViewPoolEnemyRobotGray, ViewPoolEnemyRobotGray.Pool>>();

		Container.BindMemoryPool<ViewPoolDamageBullet, ViewPoolDamageBullet.Pool>()
			.WithInitialSize(_settings.GetPoolItem(PoolItemType.Bullet).Count)
			.FromComponentInNewPrefab(_settings.GetPoolItem(PoolItemType.Bullet).ItemGameObject)
			.UnderTransform(_containerDefaultElementPrefabs);

		Container.BindFactory<Transform, PresenterPoolDamageBullet, PresenterPoolDamageBullet.Factory>()
			.FromFactory<PooledViewPresenterFactory<PresenterPoolDamageBullet, ViewPoolDamageBullet, ViewPoolDamageBullet.Pool>>();
	}

	private void InstallPresenters()
	{
		Container.BindViewController<ViewAstronaut, PresenterAstronaut>(
			_settings.ViewAstronaut, _containerCharacterSpawn);
	}
}