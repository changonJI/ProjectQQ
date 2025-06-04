using System.Diagnostics;
using UnityEngine;

namespace QQ
{
    /// <summary>
    /// 빌드시 ENABLE_LOG 매크로가 정의되면 UnityEngine.Debug.Log, LogWarning, LogError를 호출하는 유틸리티 클래스입니다.
    /// 빌드시 ENABLE_LOG를 
    /// </summary>
    public static class LogHelper
    {
        [Conditional("ENABLE_LOG")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [Conditional("ENABLE_LOG")]
        public static void Log(object message, Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogWarning(object message, Object context)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(object message, Object context)
        {
            UnityEngine.Debug.LogError(message, context);
        }
    }

}
