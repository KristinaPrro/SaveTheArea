public class SignalGameResults
{
	public GameResultsData GameResultsData { get; private set; }
	public bool IsWin { get; private set; }

	public SignalGameResults(bool isWin, GameResultsData gameResultsData)
	{
		GameResultsData = gameResultsData;
		IsWin = isWin;
	}
}