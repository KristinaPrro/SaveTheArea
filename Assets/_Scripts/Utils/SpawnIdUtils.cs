public static class SpawnIdUtils
{
	private static int _index;
	public static int GetNext()
	{
		return _index++;
	}
}