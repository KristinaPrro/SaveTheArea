public class SignalEnemyDamage
{
	public int Damage { get; set; }
	public int DamageElementId { get; set; }
	public int EnemyId { get; set; }
	
	public SignalEnemyDamage(int damageElementId, int enemyId, int damage)
	{
		DamageElementId = damageElementId;
		EnemyId = enemyId;
		Damage = damage;
	}
}