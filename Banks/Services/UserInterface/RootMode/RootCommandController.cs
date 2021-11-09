using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.Start;

namespace Banks.Services.UserInterface.RootMode
{
    public class RootCommandController : IModeCommandController
    {
        private const string Name = "Root";
        private readonly ICentralBank _centralBank;
        private readonly List<IBankCommand> _bankCommands;

        public RootCommandController(ICentralBank centralBank)
        {
            _centralBank = centralBank;
            _bankCommands = new List<IBankCommand>();
            FillBankCommands();
        }

        public string GetModeName()
        {
            return Name;
        }

        public void Run()
        {
            Console.WriteLine($"Successfully changed mode to {Name}");
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
                    Console.WriteLine($"Could not find a command with name {command}. Write Help to see all commands");
                    continue;
                }

                if (requiredCommand.GetName().Equals(new ExitCommand().GetName()))
                {
                    break;
                }

                requiredCommand.Execute();
            }

            new Run(_centralBank).Execute();
        }

        private void FillBankCommands()
        {
            _bankCommands.Add(new RootHelpCommand(_bankCommands));
            _bankCommands.Add(new AddBankCommand(_centralBank));
            _bankCommands.Add(new ShowBanksCommand(_centralBank));
            _bankCommands.Add(new ShowAllAccountsCommand(_centralBank));
            _bankCommands.Add(new CancelTransactionCommand(_centralBank));
            _bankCommands.Add(new ChangeBankConditionsCommand(_centralBank));
            _bankCommands.Add(new ExitCommand());
        }

        private IBankCommand FindCommand(string commandName)
        {
            return _bankCommands.FirstOrDefault(p => p.GetName().Equals(commandName));
        }
    }
}