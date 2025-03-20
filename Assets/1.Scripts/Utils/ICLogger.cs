using System;
using UnityEngine;

public class ICLogger
{
    public static bool isDevelop = true;

    public static void Log(object log)
    {
        if (isDevelop)
        {
            Debug.Log(log);
        }
    }

    public static void LogError(object error)
    {
        if (isDevelop)
        {
            Debug.LogError(error);
        }
    }

    public static void LogError(object error, UnityEngine.Object obj)
    {
        if (isDevelop)
        {
            Debug.LogError(error, obj);
        }
    }

    public static void LogException(Exception error)
    {
        if (isDevelop)
        {
            Debug.LogException(error);
        }
    }

    public static void LogWarning(object warning)
    {
        if (isDevelop)
        {
            Debug.LogWarning(warning);
        }
    }
}