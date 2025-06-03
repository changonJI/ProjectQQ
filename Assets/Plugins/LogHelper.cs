using System.Diagnostics;
using UnityEngine;

namespace QQ
{
    /// <summary>
    /// ����� ENABLE_LOG ��ũ�ΰ� ���ǵǸ� UnityEngine.Debug.Log, LogWarning, LogError�� ȣ���ϴ� ��ƿ��Ƽ Ŭ�����Դϴ�.
    /// ����� ENABLE_LOG�� 
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
