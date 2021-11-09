using System;

namespace Banks.Services.UserInterface.Start
{
    public class ExitCommandController : IModeCommandController
    {
        private const string Name = "Exit";
        public string GetModeName()
        {
            return Name;
        }

        public void Run()
        {
            Environment.Exit(0);
        }
    }
}