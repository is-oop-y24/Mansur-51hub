using System;
using System.Linq;
using Banks.Services.CentralBank;

namespace Banks.Services.UserInterface.Start
{
    public class Run
    {
        private readonly ICentralBank _centralBank;

        public Run(ICentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public static string GetApplicationMode()
        {
            const string modeChoosing = "Please, choose the application mode";
            Console.WriteLine(modeChoosing);
            var mods = new ApplicationMode().ModeNames.ToList();
            mods.ForEach(Console.WriteLine);
            while (true)
            {
                string mode = Console.ReadLine();
                string requiredModName = mods.FirstOrDefault(p => p.Equals(mode));
                if (requiredModName == null)
                {
                    Console.WriteLine("Could not find mode. Please try again");
                    continue;
                }

                return mode;
            }
        }

        public void TransferControlToController(string mode)
        {
            var modeCommandController = new ModeControllers(_centralBank);
            modeCommandController.TransferControlToController(mode);
        }

        public void Execute()
        {
            string mode = GetApplicationMode();
            TransferControlToController(mode);
        }
    }
}