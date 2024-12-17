using UnityEngine;

public class ModelPlayerSpawnDamageElement
{
	private readonly GameSettings _gameSettings;
	private readonly ModelPlayerDamageElements _modelPlayerDamageElements;
	private readonly PresenterPoolDamageBullet.Factory _presenterPoolDamageBulletFactory;

	public ModelPlayerSpawnDamageElement(
		GameSettings gameSettings,
		PresenterPoolDamageBullet.Factory presenterPoolDamageBulletFactory,
		ModelPlayerDamageElements modelPlayerDamageElementObjects) : base()
	{
		_gameSettings = gameSettings;
		_presenterPoolDamageBulletFactory = presenterPoolDamageBulletFactory;
		_modelPlayerDamageElements = modelPlayerDamageElementObjects;
	}

	public IDamageElement CreateDamageElement(Transform containerEnemySpawn)
	{
		var presenter = _presenterPoolDamageBulletFactory.Create(containerEnemySpawn);
		presenter.SetData(
			_gameSettings.CharacterBulletSpeed,
			_gameSettings.CharacterDamagePerShot,
			IdHandler.GetNext());

		_modelPlayerDamageElements.AddElement(presenter);

		return presenter;
	}
}
