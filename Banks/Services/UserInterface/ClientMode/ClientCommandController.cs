using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.RootMode;
using Banks.Services.UserInterface.Start;

namespace Banks.Services.UserInterface.ClientMode
{
    public class ClientCommandController : IModeCommandController
    {
        private const string Name = "Client";
        private readonly ICentralBank _centralBank;
        private readonly List<IBankCommand> _bankCommands;

        public ClientCommandController(ICentralBank centralBank)
        {
            _centralBank = centralBank;
            _bankCommands = new List<IBankCommand>();
        }

        public string GetModeName()
        {
            return Name;
        }

        public void Run()
        {
            SetUp setUp = new Authorization().Execute(_centralBank);
            Console.WriteLine($"Successfully changed mode to {Name}");
            FillBankCommands(setUp);
            while (true)
            {
                string command = Console.ReadLine();
                if (command is { Length: 0 })
                {
                    continue;
                }

                IBankCommand requiredCommand = FindCommand(command);
                if (requiredCommand == null)
                {
                    Console.WriteLine($"Could not find a command with name {command}. Type Help to see all commands");
                    continue;
                }

                requiredCommand.Execute();

                if (requiredCommand.GetName().Equals(new ExitCommand().GetName()))
                {
                    break;
                }
            }

            new Run(_centralBank).Execute();
        }

        private void FillBankCommands(SetUp setUp)
        {
            _bankCommands.Add(new ClientHelpCommand(_bankCommands));
            _bankCommands.Add(new CreateDepositAccountCommand(setUp, _centralBank));
            _bankCommands.Add(new CreateDebitAccountCommand(setUp, _centralBank));
            _bankCommands.Add(new CreateCreditAccountCommand(setUp, _centralBank));
            _bankCommands.Add(new TopUpBankAccountCommand(setUp));
            _bankCommands.Add(new WithdrawMoneyFromAccountCommand(setUp));
            _bankCommands.Add(new TransferMoneyCommand(setUp));
            _bankCommands.Add(new SubscribeCommand(setUp));
            _bankCommands.Add(new ShowMessagesCommand(setUp));
            _bankCommands.Add(new AddPassportDataCommand(setUp));
            _bankCommands.Add(new AddAddressCommand(setUp));
            _bankCommands.Add(new ExitCommand());
        }

        private IBankCommand FindCommand(string commandName)
        {
            return _bankCommands.FirstOrDefault(p => p.GetName().Equals(commandName));
        }
    }
}