using System;
using Banks.Services.Accounts;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;

namespace Banks.Services.UserInterface.ClientMode
{
    public class CreateDepositAccountCommand : IBankCommand
    {
        private const string Name = "Create Deposit Account";
        private readonly SetUp _setUp;
        private readonly ICentralBank _centralBank;

        public CreateDepositAccountCommand(SetUp setUp, ICentralBank centralBank)
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
            DateTime time = GetAccountTerm();
            try
            {
                Guid id = _centralBank.GetIdForBankAccount();
                var account = new DepositAccount(_setUp.Client, id, time);
                _setUp.Bank.AddAccount(account);
                Console.WriteLine($"Successfully Done. Account id: {id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private DateTime GetAccountTerm()
        {
            Console.WriteLine("Please type account term in years");
            while (true)
            {
                try
                {
                    int timeInYears = Convert.ToInt32(Console.ReadLine());
                    var endDate = new DateTime(DateTime.Now.Year + timeInYears, DateTime.Now.Month, DateTime.Now.Day);
                    return endDate;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}