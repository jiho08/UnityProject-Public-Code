#define ONLOG // 디버그 끌 땐 주석 처리

namespace Code.Core
{
    public static class UnityLogger
    {
#if ONLOG && UNITY_EDITOR
        public static void Log<T>(T message) => UnityEngine.Debug.Log(message);
        public static void LogError<T>(T message) => UnityEngine.Debug.LogError(message);
        public static void LogWarning<T>(T message) => UnityEngine.Debug.LogWarning(message);
#else
        [System.Diagnostics.Conditional("NOT_DEFINED")]
        public static void Log<T>(T message) { }

        [System.Diagnostics.Conditional("NOT_DEFINED")]
        public static void LogError<T>(T message) { }

        [System.Diagnostics.Conditional("NOT_DEFINED")]
        public static void LogWarning<T>(T message) { }
#endif
    }
}