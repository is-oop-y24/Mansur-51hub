using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Services.Bank;
using Banks.Services.Client;
using Banks.Services.TransactionOperations;
using Banks.Tools;

namespace Banks.Services.Accounts
{
    public class DepositAccount : IBankAccount
    {
        private readonly IClient _client;
        private readonly Transactions _transactionsHistory;
        private readonly Guid _id;
        private readonly Payment _payment;
        private readonly PaymentAndCommissionData _paymentData;
        private readonly DateTime _accountTerm;
        private double _balance;

        public DepositAccount(IClient client, Guid id, DateTime accountTerm)
        {
            _client = client;
            _id = id;

            if (accountTerm.CompareTo(DateTime.Now) < 0)
            {
                throw new BanksException("Account term should end in future. Not in past");
            }

            _accountTerm = accountTerm;
            _transactionsHistory = new Transactions();
            _payment = new Payment();
            _paymentData = new PaymentAndCommissionData();
            _balance = 0;
        }

        public bool CouldBeNegativeBalance()
        {
            const bool couldBeNegative = false;
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
            if (_paymentData.ShouldDoPayment())
            {
                _balance += _payment.GetMonthlyPayment();
                return;
            }

            if (_paymentData.ShouldCalculateDailyPayment())
            {
                _payment.CalculateDailyPayment(_balance, fixedInterest);
            }
        }

        public void CollectCommission(double fixedCommission)
        {
            throw new NotImplementedException();
        }

        public void IncrementANumberOfNegativeBalanceUsedTimes()
        {
            throw new NotSupportedException();
        }

        public Guid GetId()
        {
            return _id;
        }

        public bool CanWithdrawOrTransferMoney()
        {
            bool canWithdrawOrTransferMoney = DateTime.Compare(DateTime.Now, _accountTerm) > 0;
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
            if (!CouldBeNegativeBalance() && double.IsNegative(newBalance))
            {
                throw new BanksException("Deposit account could not have a negative balance");
            }

            _balance = newBalance;
        }
    }
}