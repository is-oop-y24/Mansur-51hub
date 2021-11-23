using System.Collections.Generic;

namespace BackupsExtra.Loggers
{
    public interface ILogger
    {
        IReadOnlyList<string> GetMessages();
        void CreateNewMessage(string message);
    }
}