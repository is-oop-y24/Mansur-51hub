using System.Collections.Generic;

namespace Banks.Services.Client
{
    public interface IClient
    {
        string GetClientName();
        string GetClientSecondName();
        Address GetAddress();
        void AddAddress(Address address);
        public void AddPassportData(PassportData passportData);
        PassportData GetPassportData();
        bool IsSuspicious();
        IReadOnlyList<string> GetMessages();
        void AddMessage(string message);
    }
}