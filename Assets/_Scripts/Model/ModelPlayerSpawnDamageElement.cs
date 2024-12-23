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

	public IDamageElement CreateDamageElement(Transform containerSpawnDamageElement, TargetData targetData)
	{
		var presenter = _presenterPoolDamageBulletFactory.Create(_containerBulletSpawn, 
			new DamageElementData(SpawnIdUtils.GetNext(),
				_gameSettings.CharacterDamagePerShot,
				_gameSettings.CharacterBulletSpeed,
				containerSpawnDamageElement),
			targetData);

		this.Log($"Fire! ({nameof(PresenterPoolDamageBullet)})");

		_modelPlayerDamageElements.AddElement(presenter);

		return presenter;
	}
}
