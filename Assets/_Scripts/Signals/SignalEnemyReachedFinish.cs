public class SignalEnemyReachedFinish
{
	public int EnemyId { get; set; }

	public SignalEnemyReachedFinish(int enemyId)
	{
		EnemyId = enemyId;
	}
}