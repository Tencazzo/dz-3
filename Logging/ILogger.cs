namespace dz3.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(string message);
    }
}