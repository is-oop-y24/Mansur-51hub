using System;
using Banks.Services.Client;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.ClientMode
{
    public class AddPassportDataCommand : IBankCommand
    {
        private const string Name = "Add passport data";
        private readonly SetUp _setUp;

        public AddPassportDataCommand(SetUp setUp)
        {
            _setUp = setUp;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            Console.WriteLine("Please type passport series");
            string passportSeries = Console.ReadLine();
            Console.WriteLine("Please type passport number");
            string passportNumber = Console.ReadLine();

            try
            {
                _setUp.Client.AddPassportData(new PassportData(passportSeries, passportNumber));
                new SuccessfullyDoneCommand().Print();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}