using System;
using System.Collections.Generic;
using Banks.Services.Accounts;
using Banks.Services.Client;

namespace Banks.Services.Bank
{
    public interface IBank
    {
        void AddAccount(IBankAccount account);
        IReadOnlyList<IBankAccount> GetBankAccounts();
        IBankAccount GetBankAccount(Guid accountId);
        IBankAccount FindBankAccount(Guid accountId);
        void TopUpABankAccount(Guid accountId, double value);
        void TransferMoneyToAnotherAccount(Guid accountFromId, Guid accountToId, double value, IClient client);
        void WithdrawMoneyFromAccount(Guid accountId, double value, IClient client);
        void DoPayment();
        void CollectCommission();
        string GetName();
        void SubscribeAccountOwnerToGetMessages(Guid accountId);
        public void ChangeFixedInterestOnTheBalance(double interest);
        public void ChangeFixedCommission(double fixedCommission);
        BankConditions GetBankConditions();
        void CancelTransaction(int transactionId);
        bool HasTransaction(int transactionId);
    }
}
