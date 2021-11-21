using System.Collections.Generic;
using System.IO;
using System.Text;
using BackupsExtra.Loggers.LoggerConfigurations;

namespace BackupsExtra.Loggers
{
    public class FileLogger : ILogger
    {
        private readonly List<string> _messages;
        private readonly ILoggerConfiguration _configuration;
        private readonly string _filePath;

        public FileLogger(ILoggerConfiguration configuration, string filePath)
        {
            _messages = new List<string>();
            _configuration = configuration;
            _filePath = filePath;
        }

        public IReadOnlyList<string> GetMessages()
        {
            return _messages;
        }

        public void CreateNewMessage(string message)
        {
            string newMessage = $"{_configuration.GetPrefix()}: {message}";
            _messages.Add(newMessage);
            using FileStream fstream = File.Open(_filePath, FileMode.Append);
            fstream.Write(Encoding.ASCII.GetBytes(newMessage + "\n"));
            fstream.Close();
        }
    }
}