using System;
using System.Linq;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.ClientMode
{
    public class ShowMessagesCommand : IBankCommand
    {
        private const string Name = "Show messages";
        private readonly SetUp _setUp;

        public ShowMessagesCommand(SetUp setUp)
        {
            _setUp = setUp;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            try
            {
                _setUp.Client.GetMessages().ToList().ForEach(message =>
                {
                    new PrintLine().Print();
                    Console.WriteLine(message);
                    new PrintLine().Print();
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}