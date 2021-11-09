using System;
using System.Collections.Generic;
using Banks.Services.Client;
using Banks.Services.TransactionOperations;

namespace Banks.Services.Accounts
{
    public interface IBankAccount
    {
        IClient GetClient();
        double GetBalance();
        bool CouldBeNegativeBalance();
        bool CanWithdrawOrTransferMoney();
        bool IsTheAccountSuspicious();
        IReadOnlyList<ITransactionOperation> GetTransactions();
        ITransactionOperation GetTransaction(int transactionId);
        void AddTransaction(ITransactionOperation transaction);
        void CancelTransaction(ITransactionOperation transaction);
        void SetBalance(double newBalance);
        void DoPayment(double fixedInterest);
        void CollectCommission(double fixedCommission);
        void IncrementANumberOfNegativeBalanceUsedTimes();
        Guid GetId();
    }
}