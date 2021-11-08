using System;
using Banks.Services.Bank;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.RootMode
{
    public class CancelTransactionCommand : IBankCommand
    {
        private const string Name = "Cancel transaction";
        private readonly ICentralBank _centralBank;

        public CancelTransactionCommand(ICentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public string GetName()
        {
            return Name;
        }

        public void Execute()
        {
            int id = GetId();
            try
            {
                IBank bank = _centralBank.GetBankWhichMakeTransaction(id);
                bank.CancelTransaction(id);
                new SuccessfullyDoneCommand().Print();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private int GetId()
        {
            Console.WriteLine("Please type transaction id");
            while (true)
            {
                try
                {
                    int id = Convert.ToInt32(Console.ReadLine());
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