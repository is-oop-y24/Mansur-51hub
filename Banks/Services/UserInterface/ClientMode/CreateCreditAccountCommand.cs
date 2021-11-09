using System;
using Banks.Services.Accounts;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;

namespace Banks.Services.UserInterface.ClientMode
{
    public class CreateCreditAccountCommand : IBankCommand
    {
        private const string Name = "Create Credit Account";
        private readonly SetUp _setUp;
        private readonly ICentralBank _centralBank;

        public CreateCreditAccountCommand(SetUp setUp, ICentralBank centralBank)
        {
            _setUp = setUp;
            _centralBank = centralBank;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            double limit = GetLimit();
            try
            {
                Guid id = _centralBank.GetIdForBankAccount();
                var account = new CreditAccount(_setUp.Client, id, limit);
                _setUp.Bank.AddAccount(account);
                Console.WriteLine($"Successfully done. Account id: {id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private double GetLimit()
        {
            Console.WriteLine("Please type limit for account");
            while (true)
            {
                try
                {
                    double limit = Convert.ToDouble(Console.ReadLine());
                    return limit;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}