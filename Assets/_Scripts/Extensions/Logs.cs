using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Logs
{
	private static bool IsDebagMode = true;
	private readonly static Dictionary<LogChannel, (bool isActive, string color)> _types = new()
	{
			[LogChannel.Debug] = (true, "ff00ffff"),
			[LogChannel.Default] = (true, "ffffffff"),
			[LogChannel.SpawnObject] = (true, "a0ffddff"),
			[LogChannel.Moving] = (true, "ffeaa0ff"),
			[LogChannel.Animation] = (true, "b0b0b0ff"),
	};

	public static void LogDebug(this object obj,
		string message = "",
		LogChannel logType = LogChannel.Debug,
		[CallerMemberName] string callerMethodName = "")
	{
		if (!IsDebagMode)
			return;

		var channelInfo = CheckChannelInfo(logType);

		if (!channelInfo.isActive)
			return;

		Debug.LogWarningFormat(GetMessage(obj, callerMethodName, $"<color=#{channelInfo.color}>/// {message}</color>")
			.AddActualTimeLogs());
	}

	public static void Log(this object obj,
		string message = "",
		LogChannel logType = LogChannel.Default,
		[CallerMemberName] string callerMethodName = "")
	{
		Debug.LogFormat(GetMessage(obj, callerMethodName, message));
	}

	public static void LogWarning(this object obj,
		string message = "",
		LogChannel logType = LogChannel.Default,
		[CallerMemberName] string callerMethodName = "")
	{
		Debug.LogWarningFormat(GetMessage(obj, callerMethodName, message));
	}

	public static void LogError(this object obj,
		string message = "",
		[CallerMemberName] string callerMethodName = "")
	{
		Debug.LogErrorFormat(GetMessage(obj, callerMethodName, message));
	}

	private static (bool isActive, string color) CheckChannelInfo(LogChannel logChannel)
	{
		if(!_types.TryGetValue(logChannel, out var info))
		{
			Debug.LogErrorFormat($"Not found '{logChannel}'!");
			return (false, "#ff0000ff") ;
		}

		return info;
	}

	private static string GetMessage(object obj, string callerMethodName, string message)
	{
		return $"[{obj?.GetType()}] {callerMethodName}() {message}";
	}

	private static string AddActualTimeLogs(this string log)
	{
		return $"{log} \n realtimeSinceStartup: {Time.realtimeSinceStartup} frame: {Time.frameCount}";
	}
}