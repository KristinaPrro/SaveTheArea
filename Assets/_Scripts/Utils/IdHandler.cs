public static class IdHandler
{
	private static int _index;
	public static int GetNext()
	{
		return _index++;
	}
}