public class SignalPlayerDamage
{
	public int Damage { get; set; }
	public int EnemyId { get; set; }

	public SignalPlayerDamage(int damage, int enemyId)
	{
		Damage = damage;
		EnemyId = enemyId;
	}	
}