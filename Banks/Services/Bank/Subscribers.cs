using System.Collections.Generic;
using Banks.Services.Client;

namespace Banks.Services.Bank
{
    public class Subscribers : List<IClient>
    {
        public void SendMessage(string message)
        {
            ForEach(client =>
                client.AddMessage(message));
        }
    }
}