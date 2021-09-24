using MelonLoader;

namespace ActionMenuApi
{
    internal static class Logger
    {
        public static void Log(string message)
        {
#if DEBUG
            MelonLogger.Msg(message);
#endif
        }

        public static void LogWarning(string message)
        {
#if DEBUG
            MelonLogger.Warning(message);
#endif
        }

        public static void LogError(string message)
        {
#if DEBUG
            MelonLogger.Error(message);
#endif
        }
    }
}