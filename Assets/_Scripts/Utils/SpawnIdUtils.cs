public static class SpawnIdUtils
{
	static private int _index;
	static public int GetNext()
	{
		return _index++;
	}
}