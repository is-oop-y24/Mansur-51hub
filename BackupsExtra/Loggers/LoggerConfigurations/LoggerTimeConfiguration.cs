using System;

namespace BackupsExtra.Loggers.LoggerConfigurations
{
    public class LoggerTimeConfiguration : ILoggerConfiguration
    {
        private readonly string _prefix;

        public LoggerTimeConfiguration()
        {
            _prefix = $"[{DateTime.Now:G}]";
        }

        public string GetPrefix()
        {
            return _prefix;
        }
    }
}