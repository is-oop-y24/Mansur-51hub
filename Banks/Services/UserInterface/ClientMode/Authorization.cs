using System;
using Banks.Services.Bank;
using Banks.Services.CentralBank;
using Banks.Services.Client;
using Banks.Services.UserInterface.RootMode;

namespace Banks.Services.UserInterface.ClientMode
{
    public class Authorization
    {
        public SetUp Execute(ICentralBank centralBank)
        {
            Console.WriteLine("Please type your name");
            string name = Console.ReadLine();
            Console.WriteLine("Please type your second name");
            string secondName = Console.ReadLine();

            if (centralBank.GetAllBanks().Count == 0)
            {
                Console.WriteLine("Can't find Banks. So unable to work");
                Environment.Exit(0);
            }

            Console.WriteLine("Please choose bank from these");
            new ShowBanksCommand(centralBank).Execute();
            while (true)
            {
                string bankName = Console.ReadLine();
                try
                {
                    IBank bank = centralBank.GetBank(bankName);
                    return new SetUp(new RegularBankClient(name, secondName), bank);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}