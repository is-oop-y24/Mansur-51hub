using System;
using System.Collections.Generic;
using Banks.Services.Bank;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.RootMode
{
    public class AddBankCommand : IBankCommand
    {
        private const string Name = "Add Bank";
        private readonly ICentralBank _centralBank;

        public AddBankCommand(ICentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            string bankName = GetBankName();
            List<InterestDependingFromBalance> interest = GetInterests();
            while (true)
            {
                try
                {
                    Console.WriteLine("Please type fixed interest in %");
                    double fixedInterestOnBalance = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Please type fixed commission");
                    double fixedCommission = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Please type limit for suspicious accounts");
                    double limitForSuspiciousAccounts = Convert.ToDouble(Console.ReadLine());
                    var bankConditions = new BankConditions(interest, fixedInterestOnBalance, fixedCommission, limitForSuspiciousAccounts);
                    _centralBank.AddNewBank(bankConditions, bankName);
                    new SuccessfullyDoneCommand().Print();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private string GetBankName()
        {
            Console.WriteLine("Please, type bank Name");
            while (true)
            {
                string name = Console.ReadLine();
                try
                {
                    _centralBank.GetBank(name);
                }
                catch
                {
                    return name;
                }

                Console.WriteLine($"Bank with name {name} already exist");
            }
        }

        private List<InterestDependingFromBalance> GetInterests()
        {
            Console.WriteLine("Please type the number of interests depending from balance");
            int number;
            while (true)
            {
                try
                {
                    number = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (number > 0)
            {
                Console.WriteLine("Please type interests in case: {Minimal value} {Maximal value} {Interest}");
            }

            var interests = new List<InterestDependingFromBalance>();
            for (int i = 0; i < number; i++)
            {
                while (true)
                {
                    try
                    {
                        string[] line = Console.ReadLine()?.Split();
                        if (line == null) continue;
                        double minValue = Convert.ToDouble(line[0]);
                        double maxValue = Convert.ToDouble(line[1]);
                        double interest = Convert.ToDouble(line[2]);
                        var interestDependingFromBalance = new InterestDependingFromBalance(minValue, maxValue, interest);
                        interests.Add(interestDependingFromBalance);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return interests;
        }
    }
}