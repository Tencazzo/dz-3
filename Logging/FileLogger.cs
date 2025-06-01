using System;
using System.IO;

namespace dz3.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string logFilePath;

        public FileLogger()
        {
            var logsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dz3", "Logs");
            Directory.CreateDirectory(logsDirectory);

            var fileName = $"log_{DateTime.Now:yyyy-MM-dd}.txt";
            logFilePath = Path.Combine(logsDirectory, fileName);
        }

        public void Log(string message)
        {
            try
            {
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Игнорируем ошибки логирования
            }
        }

        public void LogError(string message)
        {
            Log($"ERROR: {message}");
        }
    }
}