using System;
using System.Collections.Generic;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.ClientMode
{
    public class ClientHelpCommand : IBankCommand
    {
        private readonly List<IBankCommand> _clientCommands;

        public ClientHelpCommand(List<IBankCommand> bankCommands)
        {
            _clientCommands = bankCommands;
        }

        public string GetName()
        {
            return "Help";
        }

        public void Execute()
        {
            new PrintLine().Print();
            _clientCommands.ForEach(command =>
            {
                Console.WriteLine(command.GetName());
            });
            new PrintLine().Print();
        }
    }
}