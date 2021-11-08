using System;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.ClientMode
{
    public class TopUpBankAccountCommand : IBankCommand
    {
        private const string Name = "Top up account";
        private readonly SetUp _setUp;

        public TopUpBankAccountCommand(SetUp setUp)
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
            double value = GetValue();
            try
            {
                _setUp.Bank.TopUpABankAccount(id, value);
                new SuccessfullyDoneCommand().Print();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private double GetValue()
        {
            Console.WriteLine("Please type value");
            while (true)
            {
                try
                {
                    double value = Convert.ToDouble(Console.ReadLine());
                    return value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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