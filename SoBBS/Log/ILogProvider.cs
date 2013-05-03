namespace Sobbs.Log
{
    public interface ILogProvider
    {
        void Log(LogLevel level, string message);
    }
}
