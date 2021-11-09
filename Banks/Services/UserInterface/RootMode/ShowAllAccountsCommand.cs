using System;
using System.Linq;
using Banks.Services.CentralBank;
using Banks.Services.Client;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.RootMode
{
    public class ShowAllAccountsCommand : IBankCommand
    {
        private const string Name = "Show all accounts";
        private readonly ICentralBank _centralBank;

        public ShowAllAccountsCommand(ICentralBank centralBank)
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
            Console.WriteLine($"Banks number: {banks.Count}");
            banks.ForEach(bank =>
            {
                new PrintLine().Print();
                Console.WriteLine($"Bank name: {bank.GetName()}");
                var accounts = bank.GetBankAccounts().ToList();
                Console.WriteLine($"Accounts count {accounts.Count}");
                accounts.ForEach(account =>
                {
                    new PrintLine().Print();
                    Console.WriteLine($"Type: {account.GetType().Name}");
                    Console.WriteLine($"Id: {account.GetId()}");
                    Console.WriteLine($"Balance: {account.GetBalance()}");
                    Console.WriteLine($"Client:");
                    new PrintLine().Print();
                    IClient client = account.GetClient();
                    Console.WriteLine($"Name: {client.GetClientName()}");
                    Console.WriteLine($"Second Name: {client.GetClientSecondName()}");
                    Console.WriteLine($"Is suspicious: {client.IsSuspicious()}");
                });
            });
        }
    }
}