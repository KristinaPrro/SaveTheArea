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
	private Transform _containerDefaultElementPrefabs;

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
		//Container.DeclareSignal<SignalPlayerDamage>();
	}

	private void InstallModels()
	{
		Container.Bind(typeof(IInputModel), typeof(ITickable))
			.To<DesktopInputModel>()
			.AsSingle()
			.NonLazy();

		Container.BindInterfacesAndSelfTo<EnemySpawnModel>()
			.AsSingle()
			.WithArguments(_containerEnemySpawn)
			.NonLazy();

		//Container.BindInterfacesAndSelfTo<FinishModel>().NonLazy();
		//Container.BindInterfacesAndSelfTo<PlayerModel>().NonLazy();
	}

	private void InstallPools()
	{
		Container.BindMemoryPool<ViewPoolRobotGray, ViewPoolRobotGray.Pool>()
			.WithInitialSize(_settings.GetPoolItem(PoolItemType.RobotGray).Count)
			.FromComponentInNewPrefab(_settings.GetPoolItem(PoolItemType.RobotGray).ItemGameObject)
			.UnderTransform(_containerDefaultElementPrefabs);

		Container.BindFactory<Transform, PresenterPoolRobotGray, PresenterPoolRobotGray.Factory>()
			.FromFactory<PooledViewPresenterFactory<PresenterPoolRobotGray, ViewPoolRobotGray, ViewPoolRobotGray.Pool>>();
	}

	private void InstallPresenters()
	{
		Container.BindViewController<ViewAstronaut, PresenterAstronaut>(
			_settings.ViewAstronaut, _containerCharacterSpawn);
	}
}