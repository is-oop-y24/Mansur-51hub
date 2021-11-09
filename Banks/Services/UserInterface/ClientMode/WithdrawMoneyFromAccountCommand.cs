using System;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.ClientMode
{
    public class WithdrawMoneyFromAccountCommand : IBankCommand
    {
        private const string Name = "Withdraw money from account";
        private readonly SetUp _setUp;

        public WithdrawMoneyFromAccountCommand(SetUp setUp)
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
                _setUp.Bank.WithdrawMoneyFromAccount(id, value, _setUp.Client);
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