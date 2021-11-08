using Banks.Services.UserInterface.Start;

namespace Banks.Services.UserInterface.RootMode
{
    public class ExitCommand : IBankCommand
    {
        private const string Name = "Exit";

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
        }
    }
}