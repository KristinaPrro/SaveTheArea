﻿using UnityEngine;
using Zenject;

public class GameUiInstaller : MonoInstaller
{
	[SerializeField]
	private GameUiSettings _uiSettings;
	[SerializeField]
	private GameSettings _settings;
	[SerializeField]
	private Transform _containerScreenPrefabs;
	[SerializeField]
	private Transform _containerDefaultElementPrefabs;

	private void OnDestroy()
	{
		_uiSettings = null;
		_settings = null;
		_containerScreenPrefabs = null;
		_containerDefaultElementPrefabs = null;
	}

	public override void InstallBindings()
	{
		Container.BindInstances(_uiSettings);

		InstallSignals();
		InstallModels();
		InstallPools();
		InstallPresenters();
		
		//at the end
		Container.BindInterfacesAndSelfTo<ModelLevelUiInit>().AsSingle().NonLazy();
	}

	private void InstallSignals()
	{
	}

	private void InstallModels()
	{
		Container.BindInterfacesAndSelfTo<ModelLevelUi>().AsSingle().NonLazy();
		Container.BindInterfacesAndSelfTo<ModelLevelUiAutoWindowChange>().AsSingle().NonLazy();
	}

	private void InstallPools()
	{
		//UiPresenterEnemyDamageElement: UiPresenterBase<UiViewEnemyDamageElement>
		//Container.BindMemoryPool<ViewPoolDamageBullet, ViewPoolDamageBullet.Pool>()
		//	.WithInitialSize(_settings.GetPoolItem(PoolItemType.Bullet).Count)
		//	.FromComponentInNewPrefab(_settings.GetPoolItem(PoolItemType.Bullet).ItemGameObject)
		//	.UnderTransform(_containerDefaultElementPrefabs);

		//Container.BindFactory<Transform, PresenterPoolDamageBullet, PresenterPoolDamageBullet.Factory>()
		//	.FromFactory<PooledViewPresenterFactory<PresenterPoolDamageBullet, ViewPoolDamageBullet, ViewPoolDamageBullet.Pool>>();
	}

	private void InstallPresenters()
	{
		Container.BindViewController<UiViewGameStatusScreen, UiPresenterGameStatusScreen>(_uiSettings.ViewStatusScreen, _containerScreenPrefabs);
		Container.BindViewController<UiViewResultScreen, UiPresenterResultScreen>(_uiSettings.ViewResultScreen, _containerScreenPrefabs);
	}
}