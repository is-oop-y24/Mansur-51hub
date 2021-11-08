using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Services.Accounts;
using Banks.Services.Bank;
using Banks.Tools;

namespace Banks.Services.CentralBank
{
    public class CentralBank : ICentralBank
    {
        private readonly BankAccountIdGenerator _accountIdGenerator;
        private readonly TransactionIdGenerator _transactionIdGenerator;
        private readonly List<IBank> _banks;

        public CentralBank()
        {
            _accountIdGenerator = new BankAccountIdGenerator();
            _transactionIdGenerator = new TransactionIdGenerator();
            _banks = new List<IBank>();
        }

        public Guid GetIdForBankAccount()
        {
            return _accountIdGenerator.GenerateId();
        }

        public int GetIdForTransaction()
        {
            return _transactionIdGenerator.GenerateId();
        }

        public void AddNewBank(BankConditions bankConditions, string name)
        {
            if (FindBank(name) != null)
            {
                throw new BanksException($"Bank with name {name} already exist");
            }

            _banks.Add(new Bank.Bank(bankConditions, this, name));
        }

        public IBankAccount GetBankAccount(Guid accountId)
        {
            IBank requiredBank = _banks.FirstOrDefault(p => p.FindBankAccount(accountId) != null);
            if (requiredBank == null)
            {
                throw new BanksException($"Could not find account with id {accountId}");
            }

            return requiredBank.GetBankAccount(accountId);
        }

        public IBank GetBank(string name)
        {
            IBank requiredBank = FindBank(name);
            if (requiredBank == null)
            {
                throw new BanksException($"Could not find bank with name {name}");
            }

            return requiredBank;
        }

        public IReadOnlyList<IBank> GetAllBanks()
        {
            return _banks;
        }

        public IBank GetBankWhichMakeTransaction(int transactionId)
        {
            IBank requiredBank = _banks.FirstOrDefault(p => p.HasTransaction(transactionId));
            if (requiredBank == null)
            {
                throw new BanksException($"Could not find transaction with id {transactionId}");
            }

            return requiredBank;
        }

        public void NotifyBanksAboutPaymentAndCommission()
        {
            _banks.ForEach(bank =>
            {
                bank.DoPayment();
            });
            _banks.ForEach(bank =>
            {
                bank.CollectCommission();
            });
        }

        private IBank FindBank(string name)
        {
            return _banks.FirstOrDefault(p => p.GetName().Equals(name));
        }
    }
}