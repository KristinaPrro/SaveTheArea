﻿using System.Runtime.CompilerServices;
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

		var channelInfo = LogsUtils.GetChannelInfo(logChannel);

		if (!channelInfo.isActive)
			return;

		Debug.LogWarningFormat(GetMessage(obj, callerMethodName, $"<color=#{channelInfo.color}> [Debug] {message}</color>")
			.AddActualTimeLogs());
	}

	public static void Log(this object obj,
		string message = "",
		LogChannel logChannel = LogChannel.Default,
		[CallerMemberName] string callerMethodName = "")
	{
		Debug.LogFormat(GetMessage(obj, callerMethodName, $"[Log] {message}"));
	}

	public static void LogWarning(this object obj,
		string message = "",
		[CallerMemberName] string callerMethodName = "")
	{
		Debug.LogWarningFormat(GetMessage(obj, callerMethodName, $"[Warning] {message}"));
	}

	public static void LogError(this object obj,
		string message = "",
		[CallerMemberName] string callerMethodName = "")
	{
		Debug.LogErrorFormat(GetMessage(obj, callerMethodName, $"[Error] {message}").AddActualTimeLogs());
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