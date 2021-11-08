using System;
using Banks.Services.Client;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.ClientMode
{
    public class AddAddressCommand : IBankCommand
    {
        private const string Name = "Add address";
        private readonly SetUp _setUp;

        public AddAddressCommand(SetUp setUp)
        {
            _setUp = setUp;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            Console.WriteLine("Please type city name");
            string cityName = Console.ReadLine();
            Console.WriteLine("Please type street name");
            string streetName = Console.ReadLine();
            Console.WriteLine("Please type house number");
            string houseNumber = Console.ReadLine();
            try
            {
                _setUp.Client.AddAddress(new Address(cityName, streetName, houseNumber));
                new SuccessfullyDoneCommand().Print();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}