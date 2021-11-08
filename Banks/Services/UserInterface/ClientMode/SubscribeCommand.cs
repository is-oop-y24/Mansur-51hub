using System;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.ClientMode
{
    public class SubscribeCommand : IBankCommand
    {
        private const string Name = "Subscribe account";
        private readonly SetUp _setUp;

        public SubscribeCommand(SetUp setUp)
        {
            _setUp = setUp;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            Guid id = GetId();
            try
            {
                _setUp.Bank.SubscribeAccountOwnerToGetMessages(id);
                new SuccessfullyDoneCommand().Print();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private Guid GetId()
        {
            Console.WriteLine("Please type account id");
            while (true)
            {
                try
                {
                    var id = Guid.Parse(Console.ReadLine() ?? string.Empty);
                    return id;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}