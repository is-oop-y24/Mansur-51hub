using System;
using System.Collections.Generic;
using Banks.Services.UserInterface.Start;
using Banks.Services.UserInterface.Tools;

namespace Banks.Services.UserInterface.RootMode
{
    public class RootHelpCommand : IBankCommand
    {
        private readonly List<IBankCommand> _rootCommands;

        public RootHelpCommand(List<IBankCommand> bankCommands)
        {
            _rootCommands = bankCommands;
        }

        public string GetName()
        {
            return "Help";
        }

        public void Execute()
        {
            new PrintLine().Print();
            _rootCommands.ForEach(command =>
            {
                Console.WriteLine(command.GetName());
            });
            new PrintLine().Print();
        }
    }
}