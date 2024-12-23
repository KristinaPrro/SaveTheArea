public struct GameResultsData
{
	public int CurrentPlayerHealth { get; private set; }
	public int MaxPlayerHealth { get; private set; }
	public int CurrentEnemyCount { get; private set; }
	public int MaxEnemyCount { get; private set; }

	public GameResultsData(int currentPlayerHealth, int maxPlayerHealth, int currentEnemyCount, int maxEnemyCount)
	{
		CurrentPlayerHealth = currentPlayerHealth;
		MaxPlayerHealth = maxPlayerHealth;
		CurrentEnemyCount = currentEnemyCount;
		MaxEnemyCount = maxEnemyCount;
	}
}