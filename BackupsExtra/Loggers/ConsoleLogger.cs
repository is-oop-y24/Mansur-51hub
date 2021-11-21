using System;
using System.Collections.Generic;
using BackupsExtra.Loggers.LoggerConfigurations;

namespace BackupsExtra.Loggers
{
    public class ConsoleLogger : ILogger
    {
        private readonly List<string> _messages;
        private readonly ILoggerConfiguration _configuration;

        public ConsoleLogger(ILoggerConfiguration configuration)
        {
            _messages = new List<string>();
            _configuration = configuration;
        }

        public IReadOnlyList<string> GetMessages()
        {
            return _messages;
        }

        public void CreateNewMessage(string message)
        {
            string newMessage = $"{_configuration.GetPrefix()}: {message}";
            _messages.Add(newMessage);
            Console.WriteLine(newMessage);
        }
    }
}