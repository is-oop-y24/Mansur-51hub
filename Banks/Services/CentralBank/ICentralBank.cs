using System;
using System.Collections.Generic;
using Banks.Services.Accounts;
using Banks.Services.Bank;

namespace Banks.Services.CentralBank
{
    public interface ICentralBank
    {
        Guid GetIdForBankAccount();
        int GetIdForTransaction();
        void AddNewBank(BankConditions bankConditions, string name);
        IBankAccount GetBankAccount(Guid accountId);
        IBank GetBank(string name);
        IReadOnlyList<IBank> GetAllBanks();
        IBank GetBankWhichMakeTransaction(int transactionId);
        void NotifyBanksAboutPaymentAndCommission();
    }
}