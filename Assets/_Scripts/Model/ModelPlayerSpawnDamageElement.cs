using System.Collections.Generic;
using UnityEngine;

public class ModelPlayerSpawnDamageElement
{
	private readonly GameSettings _gameSettings;
	private readonly Transform _containerBulletSpawn;
	private readonly ModelPlayerDamageElements _modelPlayerDamageElements;
	private readonly PresenterPoolDamageBullet.Factory _presenterPoolDamageBulletFactory;

	public ModelPlayerSpawnDamageElement(
		GameSettings gameSettings,
		Transform container,
		PresenterPoolDamageBullet.Factory presenterPoolDamageBulletFactory,
		ModelPlayerDamageElements modelPlayerDamageElementObjects) : base()
	{
		_gameSettings = gameSettings;
		_containerBulletSpawn = container;
		_presenterPoolDamageBulletFactory = presenterPoolDamageBulletFactory;
		_modelPlayerDamageElements = modelPlayerDamageElementObjects;
	}

	public IDamageElement CreateDamageElement()
	{
		var presenter = _presenterPoolDamageBulletFactory.Create(_containerBulletSpawn);
		presenter.SetData(
			_gameSettings.CharacterBulletSpeed,
			_gameSettings.CharacterDamagePerShot,
			IdHandler.GetNext());

		_modelPlayerDamageElements.AddElement(presenter);

		return presenter;
	}
}
