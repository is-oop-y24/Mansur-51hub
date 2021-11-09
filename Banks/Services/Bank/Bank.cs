using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Services.Accounts;
using Banks.Services.CentralBank;
using Banks.Services.Client;
using Banks.Services.TransactionOperations;
using Banks.Tools;

namespace Banks.Services.Bank
{
    public class Bank : IBank
    {
        private readonly BankConditions _bankConditions;
        private readonly List<IBankAccount> _bankAccounts;
        private readonly ICentralBank _centralBank;
        private readonly Subscribers _subscribers;

        public Bank(BankConditions bankConditions, ICentralBank centralBank, string name)
        {
            _bankConditions = bankConditions;
            _bankAccounts = new List<IBankAccount>();
            _centralBank = centralBank;
            _subscribers = new Subscribers();
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }
        public void AddAccount(IBankAccount account)
        {
            _bankAccounts.Add(account);
        }

        public IReadOnlyList<IBankAccount> GetBankAccounts()
        {
            return _bankAccounts;
        }

        public IBankAccount GetBankAccount(Guid accountId)
        {
            IBankAccount bankAccount = FindBankAccount(accountId);
            if (bankAccount == null)
            {
                throw new BanksException($"Could not find account with id {accountId} in bank {Name}");
            }

            return bankAccount;
        }

        public IBankAccount FindBankAccount(Guid accountId)
        {
            return _bankAccounts.FirstOrDefault(p => p.GetId().Equals(accountId));
        }

        public void TopUpABankAccount(Guid accountId, double value)
        {
            IBankAccount bankAccount = GetBankAccount(accountId);
            var operation = new TopUpABankAccountOperation(bankAccount, value, _centralBank.GetIdForTransaction());
            operation.Execute();
        }

        public void TransferMoneyToAnotherAccount(Guid accountFromId, Guid accountToId, double value, IClient client)
        {
            IBankAccount bankAccountFrom = GetBankAccount(accountFromId);

            if (!bankAccountFrom.GetClient().Equals(client))
            {
                throw new BanksException("Permission denied");
            }

            IBankAccount bankAccountTo = _centralBank.GetBankAccount(accountToId);
            var operation = new TransferMoneyToAnotherAccountOperation(value, bankAccountFrom, bankAccountTo, _bankConditions, _centralBank.GetIdForTransaction(), _centralBank.GetIdForTransaction());
            operation.Execute();
        }

        public void WithdrawMoneyFromAccount(Guid accountId, double value, IClient client)
        {
            IBankAccount bankAccount = GetBankAccount(accountId);

            if (!bankAccount.GetClient().Equals(client))
            {
                throw new BanksException("Permission denied");
            }

            var operation = new WithdrawMoneyFromTheAccountOperation(bankAccount, value, _bankConditions, _centralBank.GetIdForTransaction());
            operation.Execute();
        }

        public void DoPayment()
        {
            _bankAccounts.ForEach(account =>
                account.DoPayment(_bankConditions.FixedInterestOnTheBalance));
        }

        public void CollectCommission()
        {
            _bankAccounts.ForEach(account =>
                {
                    account.CollectCommission(_bankConditions.FixedCommission);
                });
        }

        public string GetName()
        {
            return Name;
        }

        public void SubscribeAccountOwnerToGetMessages(Guid accountId)
        {
            IBankAccount account = GetBankAccount(accountId);
            _subscribers.Add(account.GetClient());
        }

        public void ChangeFixedInterestOnTheBalance(double interest)
        {
            _bankConditions.ChangeFixedInterestOnTheBalance(interest);
            _subscribers.SendMessage($"Interest in bank {Name} was changed to {interest}");
        }

        public void ChangeFixedCommission(double fixedCommission)
        {
            _bankConditions.ChangeFixedCommission(fixedCommission);
            _subscribers.SendMessage($"Fixed commission in bank {Name} was changed to {fixedCommission}");
        }

        public BankConditions GetBankConditions()
        {
            return _bankConditions;
        }

        public void CancelTransaction(int transactionId)
        {
            IBankAccount requiredBankAccount = _bankAccounts
                .FirstOrDefault(account => account.GetTransactions()
                    .Any(transaction => transaction.GetTransactionId().Equals(transactionId)));
            if (requiredBankAccount == null)
            {
                throw new BanksException($"Could not find transaction with id {transactionId}");
            }

            requiredBankAccount.GetTransaction(transactionId).Undo();
        }

        public bool HasTransaction(int transactionId)
        {
            return _bankAccounts
                .Any(account => account.GetTransactions()
                    .Any(transaction => transaction.GetTransactionId().Equals(transactionId)));
        }
    }
}