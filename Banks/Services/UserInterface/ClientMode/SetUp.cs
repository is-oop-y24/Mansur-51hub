using Banks.Services.Bank;
using Banks.Services.Client;

namespace Banks.Services.UserInterface.ClientMode
{
    public class SetUp
    {
        public SetUp(IClient client, IBank bank)
        {
            Client = client;
            Bank = bank;
        }

        public IClient Client { get; }
        public IBank Bank { get; }
    }
}