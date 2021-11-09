using System;

namespace Banks.Services.CentralBank
{
    public class BankAccountIdGenerator
    {
        private Guid _id;

        public BankAccountIdGenerator()
        {
            _id = Guid.Empty;
        }

        public Guid GenerateId()
        {
            _id = Guid.NewGuid();
            return _id;
        }
    }
}