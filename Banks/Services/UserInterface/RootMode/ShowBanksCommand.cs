using System;
using System.Linq;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.RootMode
{
    public class ShowBanksCommand : IBankCommand
    {
        private const string Name = "Show Banks";
        private readonly ICentralBank _centralBank;

        public ShowBanksCommand(ICentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            var banks = _centralBank.GetAllBanks().ToList();
            Console.WriteLine($"Banks number : {banks.Count}");
            banks.ForEach(bank =>
            {
                new PrintLine().Print();
                Console.WriteLine($"Name: {bank.GetName()}");
                Console.WriteLine($"Fixed interest: {bank.GetBankConditions().FixedInterestOnTheBalance}%");
                Console.WriteLine($"Fixed commission: {bank.GetBankConditions().FixedCommission}");
                Console.WriteLine($"Accounts number: {bank.GetBankAccounts().Count}");
                new PrintLine().Print();
            });
        }
    }
}