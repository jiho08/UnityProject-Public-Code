#define ONLOG // 디버그 끌 땐 주석 처리

namespace Core
{
    public static class UnityLogger
    {
#if ONLOG && UNITY_EDITOR
        public static void Log(string message) => UnityEngine.Debug.Log(message);
        public static void LogError(string message) => UnityEngine.Debug.LogError(message);
        public static void LogWarning(string message) => UnityEngine.Debug.LogWarning(message);
#else
        [System.Diagnostics.Conditional("NOT_DEFINED")]
        public static void Log(string message) { }

        [System.Diagnostics.Conditional("NOT_DEFINED")]
        public static void LogError(string message) { }

        [System.Diagnostics.Conditional("NOT_DEFINED")]
        public static void LogWarning(string message) { }
#endif
    }
}