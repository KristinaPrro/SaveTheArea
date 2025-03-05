using System.Collections.Generic;
using UnityEngine;

public static class LogsUtils
{
	public const bool IS_DEBUG_MODE = true;

	private static Dictionary<LogChannel, (bool isActive, string color)> Channels = new()
	{
		[LogChannel.Debug] = (true, "ff00ffff"),
		[LogChannel.Default] = (true, "ffffffff"),
		[LogChannel.SpawnObject] = (false, "a0ffddff"),
		[LogChannel.Moving] = (false, "ffeaa0ff"),
		[LogChannel.Animation] = (false, "6f6f6fff"),
		[LogChannel.Todo] = (true, "ff0000ff"),
		[LogChannel.Attack] = (true, "ffffffff"),
	};

	public static (bool isActive, string color) GetChannelInfo(LogChannel logChannel)
	{
		if (!Channels.TryGetValue(logChannel, out var info))
		{
			Debug.LogErrorFormat($"Not found '{logChannel}'!");
			return (false, "#ff0000ff");
		}

		return info;
	}
}