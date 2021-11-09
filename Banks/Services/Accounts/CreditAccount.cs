using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Services.Bank;
using Banks.Services.Client;
using Banks.Services.TransactionOperations;
using Banks.Tools;

namespace Banks.Services.Accounts
{
    public class CreditAccount : IBankAccount
    {
        private readonly IClient _client;
        private readonly Transactions _transactionsHistory;
        private readonly Guid _id;
        private double _balance;
        private double _negativeBalanceUsedTimes;

        public CreditAccount(IClient client, Guid id, double limit)
        {
            _client = client;
            _id = id;
            _transactionsHistory = new Transactions();
            _balance = limit;
            _negativeBalanceUsedTimes = 0;
        }

        public bool CouldBeNegativeBalance()
        {
            const bool couldBeNegative = true;
            return couldBeNegative;
        }

        public double GetBalance()
        {
            return _balance;
        }

        public IClient GetClient()
        {
            return _client;
        }

        public void DoPayment(double fixedInterest)
        {
        }

        public void CollectCommission(double fixedCommission)
        {
            _balance -= fixedCommission * _negativeBalanceUsedTimes;
            _negativeBalanceUsedTimes = 0;
        }

        public void IncrementANumberOfNegativeBalanceUsedTimes()
        {
            _negativeBalanceUsedTimes++;
        }

        public Guid GetId()
        {
            return _id;
        }

        public bool CanWithdrawOrTransferMoney()
        {
            const bool canWithdrawOrTransferMoney = true;
            return canWithdrawOrTransferMoney;
        }

        public bool IsTheAccountSuspicious()
        {
            return _client.IsSuspicious();
        }

        public IReadOnlyList<ITransactionOperation> GetTransactions()
        {
            return _transactionsHistory;
        }

        public ITransactionOperation GetTransaction(int transactionId)
        {
            ITransactionOperation requiredTransaction = _transactionsHistory.FirstOrDefault(p => p.GetTransactionId().Equals(transactionId));
            if (requiredTransaction == null)
            {
                throw new BanksException($"Could not find transaction with id {transactionId} in account {_id}");
            }

            return requiredTransaction;
        }

        public void AddTransaction(ITransactionOperation transaction)
        {
            _transactionsHistory.Add(transaction);
        }

        public void CancelTransaction(ITransactionOperation transaction)
        {
            _transactionsHistory.Remove(transaction);
        }

        public void SetBalance(double newBalance)
        {
            _balance = newBalance;
        }
    }
}