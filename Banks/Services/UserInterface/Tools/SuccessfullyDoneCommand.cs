using System;

namespace Banks.Services.UserInterface.Tools
{
    public class SuccessfullyDoneCommand
    {
        public void Print()
        {
            const string message = "Successfully done";
            Console.WriteLine(message);
            new PrintLine().Print();
        }
    }
}