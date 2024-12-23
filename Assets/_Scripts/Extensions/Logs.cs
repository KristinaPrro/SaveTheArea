using System.Runtime.CompilerServices;
using UnityEngine;

public static class Logs
{
	public static void LogDebug(this object obj,
		string message,
		LogChannel logChannel = LogChannel.Debug,
		[CallerMemberName] string callerMethodName = "")
	{
		if (!LogsUtils.IS_DEBUG_MODE)
			return;

		var channelInfo = CheckChannelInfo(logChannel);

		if (!channelInfo.isActive)
			return;

		Debug.LogWarningFormat(GetMessage(obj, callerMethodName, $"<color=#{channelInfo.color}>/// {message}</color>")
			.AddActualTimeLogs());
	}

	public static void Log(this object obj,
		string message = "",
		LogChannel logChannel = LogChannel.Default,
		[CallerMemberName] string callerMethodName = "")
	{
		Debug.LogFormat(GetMessage(obj, callerMethodName, message));
	}

	public static void LogWarning(this object obj,
		string message = "",
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

	private static string GetMessage(object obj, string callerMethodName, string message)
	{
		return $"[{obj?.GetType()}] {callerMethodName}() {message}";
	}

	private static string AddActualTimeLogs(this string log)
	{
		return $"{log} \n realtimeSinceStartup: {Time.realtimeSinceStartup} frame: {Time.frameCount}";
	}
}