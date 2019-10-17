#define PRINT_PREFIX

#if !UNITY_EDITOR
#define PRINT_TIME
#endif

#if UNITY_EDITOR || DEVELOPMENT_BUILD
#define PRINT_DEBUG
#endif

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Laser
{
    public enum LogLevel
    {
        Debug,
        Trace,
        Info,
        Warning,
        Error,
        Fatal
    }

    public class Logger
    {
        public string Category
        { get; private set; }

        public Logger(string category)
        {
            Category = category;
        }

        public virtual void WriteRaw(string format, params object[] args)
        {
            WriteRaw(String.Format(format, args), UnityEngine.LogType.Log);
        }

        public virtual void WriteRaw(string msg, UnityEngine.LogType logType)
        {
#if PRINT_TIME
            if (Category != null)
            {
                msg = $"<{DateTime.Now.ToString("hh:mm:ss.fff")}> <{Category}> {msg}";
            }
            else
            {
                msg = $"<{DateTime.Now.ToString("hh:mm:ss.fff")}> {msg}";
            }
#else
            if (Category != null)
            {
                msg = $"<{Category}> {msg}";
            }
#endif

            UnityEngine.Debug.unityLogger.Log(logType, msg);
        }

        public virtual void Write(object obj)
        {
            if (obj == null)
            {
                Write("<null>");
            }

            Write(obj.ToString());
        }

        public virtual void Write(string format, params object[] args)
        {
            if (format == null)
            {
                Write("<null>");
            }

            Write(String.Format(format, args));
        }

        public virtual void Write(LogLevel level, string format, params object[] args)
        {
            if (format == null)
            {
                Write(level, "<null>");
            }

            Write(level, String.Format(format, args));
        }

        public virtual void Write(string msg)
        {
            Write(LogLevel.Trace, msg);
        }

        public virtual void Info(string msg)
        {
            Write(LogLevel.Info, msg);
        }

        public virtual void Info(string format, params object[] args)
        {
            Write(LogLevel.Info, String.Format(format, args));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Debug(string msg)
        {
#if PRINT_DEBUG
            Write(LogLevel.Debug, msg);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Debug(string format, params object[] args)
        {
#if PRINT_DEBUG
            Write(LogLevel.Debug, String.Format(format, args));
#endif
        }

        public virtual void Warn(string msg)
        {
            Write(LogLevel.Warning, msg);
        }

        public virtual void Warn(string format, params object[] args)
        {
            Write(LogLevel.Warning, String.Format(format, args));
        }

        public virtual void Error(string msg)
        {
            Write(LogLevel.Error, msg);
        }

        public virtual void Error(string format, params object[] args)
        {
            Write(LogLevel.Error, String.Format(format, args));
        }

        public virtual void Fatal(string msg)
        {
            Write(LogLevel.Fatal, msg);
        }

        public virtual void Fatal(string format, params object[] args)
        {
            Write(LogLevel.Fatal, String.Format(format, args));
        }

        public virtual void Write(LogLevel level, string msg)
        {
#if !PRINT_DEBUG
		if (level == LogLevel.Debug) return;
#endif

#if PRINT_PREFIX
            var prefix =
                level == LogLevel.Trace ? "[Trace] " :
                level == LogLevel.Info ? "[Info] " :
                level == LogLevel.Debug ? "[Debug] " :
                level == LogLevel.Warning ? "[Warning] " :
                level == LogLevel.Error ? "[Error] " :
                level == LogLevel.Fatal ? "[Fatal] " :
                String.Empty;
#endif

            var unityLogType =
                level == LogLevel.Debug || level == LogLevel.Info ? UnityEngine.LogType.Log :
                level == LogLevel.Error || level == LogLevel.Fatal ? UnityEngine.LogType.Error :
                level == LogLevel.Trace ? UnityEngine.LogType.Assert :
                level == LogLevel.Warning ? UnityEngine.LogType.Warning :
                UnityEngine.LogType.Log;

#if PRINT_PREFIX
            WriteRaw(prefix + msg, unityLogType);
#else
            WriteRaw(msg, unityLogType);
#endif
        }
    }

    public static class L
    {
        private static Logger logger = new Logger(null);

        public static void WriteRaw(string format, params object[] args)
        {
            logger.WriteRaw(format, args);
        }

        private static void WriteRaw(string msg, UnityEngine.LogType logType)
        {
            logger.WriteRaw(msg, logType);
        }

        public static void Write(object obj)
        {
            logger.Write(obj);
        }

        public static void Write(string format, params object[] args)
        {
            logger.Write(format, args);
        }

        public static void Write(LogLevel level, string format, params object[] args)
        {
            logger.Write(level, format, args);
        }

        public static void Write(string msg)
        {
            logger.Write(msg);
        }

        public static void Info(string msg)
        {
            logger.Info(msg);
        }

        public static void Info(string format, params object[] args)
        {
            logger.Info(format, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string msg)
        {
#if PRINT_DEBUG
            logger.Debug(msg);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string format, params object[] args)
        {
#if PRINT_DEBUG
            logger.Debug(format, args);
#endif
        }

        public static void Warn(string msg)
        {
            logger.Warn(msg);
        }

        public static void Warn(string format, params object[] args)
        {
            logger.Warn(format, args);
        }

        public static void Error(string msg)
        {
            logger.Error(msg);
        }

        public static void Error(string format, params object[] args)
        {
            logger.Error(format, args);
        }

        public static void Fatal(string msg)
        {
            logger.Fatal(msg);
        }

        public static void Fatal(string format, params object[] args)
        {
            logger.Fatal(format, args);
        }

        public static void Write(LogLevel level, string msg)
        {
#if !PRINT_DEBUG
		if (level == LogLevel.Debug) return;
#endif

#if PRINT_PREFIX
            var prefix =
                level == LogLevel.Trace ? "[Trace] " :
                level == LogLevel.Info ? "[Info] " :
                level == LogLevel.Debug ? "[Debug] " :
                level == LogLevel.Warning ? "[Warning] " :
                level == LogLevel.Error ? "[Error] " :
                level == LogLevel.Fatal ? "[Fatal] " :
                String.Empty;
#endif

            var unityLogType =
                level == LogLevel.Debug || level == LogLevel.Info ? UnityEngine.LogType.Log :
                level == LogLevel.Error || level == LogLevel.Fatal ? UnityEngine.LogType.Error :
                level == LogLevel.Trace ? UnityEngine.LogType.Assert :
                level == LogLevel.Warning ? UnityEngine.LogType.Warning :
                UnityEngine.LogType.Log;

#if PRINT_PREFIX
            WriteRaw(prefix + msg, unityLogType);
#else
            WriteRaw(msg, unityLogType);
#endif
        }
    }
}