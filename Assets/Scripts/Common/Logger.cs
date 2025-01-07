using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public static class Logger
{
    public static void Log(string message) =>
        Debug.LogFormat("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);

    public static void LogWarning(string message) =>
        Debug.LogWarningFormat("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);

    public static void LogError(string message) =>
        Debug.LogErrorFormat("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
}