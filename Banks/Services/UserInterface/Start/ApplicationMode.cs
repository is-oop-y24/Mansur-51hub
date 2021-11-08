using System.Collections.Generic;

namespace Banks.Services.UserInterface.Start
{
    public class ApplicationMode
    {
        public IReadOnlyList<string> ModeNames { get; } = new List<string>
        {
            "Root",
            "Client",
            "Exit",
        };
    }
}