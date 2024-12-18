using System.Runtime.CompilerServices;
using UnityEngine;

public static class Logs
{
	private static bool IsDebagLogs = false;

	public static void LogDebug(this object obj,
		string message = "",
		[CallerMemberName] string callerMethodName = "")
	{
		if (!IsDebagLogs)
			return;

		Debug.LogWarningFormat(GetMessage(obj, callerMethodName, $"<color=#ff00ffff>/// {message}</color>"));
	}

	public static void Log(this object obj,
		string message = "",
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
}