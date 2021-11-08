using System;
using Banks.Services.Bank;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.RootMode
{
    public class ChangeBankConditionsCommand : IBankCommand
    {
        private const string Name = "Change bank conditions";
        private readonly ICentralBank _centralBank;

        public ChangeBankConditionsCommand(ICentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            IBank bank = GetBank();
            while (true)
            {
                try
                {
                    Console.WriteLine("Please type fixed interest in %");
                    double fixedInterestOnBalance = Convert.ToDouble(Console.ReadLine());
                    bank.ChangeFixedInterestOnTheBalance(fixedInterestOnBalance);
                    Console.WriteLine("Please type fixed commission");
                    double fixedCommission = Convert.ToDouble(Console.ReadLine());
                    bank.ChangeFixedCommission(fixedCommission);
                    new SuccessfullyDoneCommand().Print();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private IBank GetBank()
        {
            Console.WriteLine("Please type bank name");

            while (true)
            {
                string bankName = Console.ReadLine();
                try
                {
                    IBank bank = _centralBank.GetBank(bankName);
                    return bank;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}