using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
	[field: Header("Enemy:")]
	[field: SerializeField]
	public int EnemyCountMin { get; private set; } = 3;
	[field: SerializeField]
	public int EnemyCountMax { get; private set; } = 10;
	[field: SerializeField]
	public float EnemyTimeOutMin { get; private set; } = 0.5f;
	[field: SerializeField]
	public float EnemyTimeOutMax { get; private set; } = 2f;
	[field: SerializeField]
	public float EnemySpeedMin { get; private set; } = 0.5f;
	[field: SerializeField]
	public float EnemySpeedMax { get; private set; } = 2f;
	[field: SerializeField]
	public int EnemyHealth { get; private set; } = 3;

	[field: Header("Character:")]
	[field: SerializeField]
	public int CharacterHealth { get; private set; } = 3;
	[field: SerializeField]
	public float CharacterRadiusFire { get; private set; } = 3.5f;
	[field: SerializeField]
	public float CharacterRateFire { get; private set; } = 0.5f;
	[field: SerializeField]
	public int CharacterDamagePerShot { get; private set; } = 5;
	[field: SerializeField]
	public float CharacterBulletSpeed { get; private set; } = 2.5f;

	[field: Space]
	[field: SerializeField]
	public List<PoolItemData> PoolItemDatas { get; private set; } = new();

	private PoolItemData GetPoolItem(PoolItemType itemType) => PoolItemDatas.Find(item => item.PoolItemType == itemType);
}