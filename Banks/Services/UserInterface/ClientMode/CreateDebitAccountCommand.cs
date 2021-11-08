using System;
using Banks.Services.Accounts;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;

namespace Banks.Services.UserInterface.ClientMode
{
    public class CreateDebitAccountCommand : IBankCommand
    {
        private const string Name = "Create Debit Account";
        private readonly SetUp _setUp;
        private readonly ICentralBank _centralBank;

        public CreateDebitAccountCommand(SetUp setUp, ICentralBank centralBank)
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
            Guid id = _centralBank.GetIdForBankAccount();
            var account = new DebitAccount(_setUp.Client, id);
            _setUp.Bank.AddAccount(account);
            Console.WriteLine($"Successfully done. Account id: {id}");
        }
    }
}