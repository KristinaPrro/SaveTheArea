public class SignalEnemyDamage
{
	public int DamageElementId { get; set; }
	public int EnemyId { get; set; }
	
	public SignalEnemyDamage(int damageElementId, int enemyId)
	{
		DamageElementId = damageElementId;
		EnemyId = enemyId;
	}
}