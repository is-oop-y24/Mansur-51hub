using System.Collections.Generic;
using System.Linq;
using Banks.Services.CentralBank;
using Banks.Services.UserInterface.ClientMode;
using Banks.Services.UserInterface.RootMode;

namespace Banks.Services.UserInterface.Start
{
    public class ModeControllers
    {
        private readonly List<IModeCommandController> _commandControllers;

        public ModeControllers(ICentralBank centralBank)
        {
            _commandControllers = new List<IModeCommandController> { new RootCommandController(centralBank), new ClientCommandController(centralBank), new ExitCommandController() };
        }

        public void TransferControlToController(string name)
        {
             _commandControllers
                 .First(p => p.GetModeName().Equals(name))
                 .Run();
        }
    }
}