using System;
using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Services.Client
{
    public class RegularBankClient : IClient
    {
        private readonly string _name;
        private readonly string _secondName;
        private readonly List<string> _messages;
        private PassportData _passportData;
        private Address _address;

        public RegularBankClient(string name, string secondName)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _secondName = secondName ?? throw new ArgumentNullException(nameof(secondName));
            _messages = new List<string>();
        }

        public void AddPassportData(PassportData passportData)
        {
            if (_passportData != null)
            {
                throw new BanksException("Passport data has already been added");
            }

            _passportData = passportData;
        }

        public void AddAddress(Address address)
        {
            if (_address != null)
            {
                throw new BanksException("Address has already been added");
            }

            _address = address;
        }

        public Address GetAddress()
        {
            if (_address == null)
            {
                throw new BanksException("Client didn't add address");
            }

            return _address;
        }

        public string GetClientName()
        {
            return _name;
        }

        public string GetClientSecondName()
        {
            return _secondName;
        }

        public PassportData GetPassportData()
        {
            if (_passportData == null)
            {
                throw new BanksException("Client didn't add passport data");
            }

            return _passportData;
        }

        public bool IsSuspicious()
        {
            return _address == null || _passportData == null;
        }

        public IReadOnlyList<string> GetMessages()
        {
            return _messages;
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }
    }
}