using System;
using System.IO;

namespace dz3.Logging
{
    public class FileLogger : ILogger, IDisposable
    {
        private readonly string logFilePath;
        private readonly object lockObject = new object();
        private bool disposed = false;

        public FileLogger(string category = "Application")
        {
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            logFilePath = Path.Combine(logDirectory, $"TaskManager_{DateTime.Now:yyyyMMdd}.log");
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARN", message);
        }

        public void LogError(string message, Exception exception = null)
        {
            var fullMessage = exception != null ? $"{message} | Exception: {exception}" : message;
            WriteLog("ERROR", fullMessage);
        }

        public void LogDebug(string message)
        {
            WriteLog("DEBUG", message);
        }

        private void WriteLog(string level, string message)
        {
            if (disposed) return;

            lock (lockObject)
            {
                try
                {
                    var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
                catch
                {
                    // Игнорируем ошибки записи в лог
                }
            }
        }

        public void Dispose()
        {
            disposed = true;
        }
    }
}