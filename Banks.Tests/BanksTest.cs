using System;
using System.Collections.Generic;
using Banks.Services.Accounts;
using Banks.Services.Bank;
using Banks.Services.CentralBank;
using Banks.Services.Client;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private ICentralBank _centralBank;
        private IBank _bank1;
        private IBank _bank2;
        private IClient _client1;
        private IClient _client2;
        private DebitAccount _debitAccount;
        private DepositAccount _depositAccount;
        private CreditAccount _creditAccount;

        [SetUp]
        public void SetUp()
        {
            _centralBank = new CentralBank();

            double fixedInterestOnBalance1 = 2;
            double fixedCommission1 = 3;
            double limitForSuspiciousAccounts1 = 20000;
            var conditions1 = new BankConditions(new List<InterestDependingFromBalance>(), fixedInterestOnBalance1,
                fixedCommission1, limitForSuspiciousAccounts1);
            string bankName1 = "Sber";
            _centralBank.AddNewBank(conditions1, bankName1);
            _bank1 = _centralBank.GetBank(bankName1);

            double fixedInterestOnBalance2 = 3;
            double fixedCommission2 = 5;
            double limitForSuspiciousAccounts2 = 5000;
            var conditions2 = new BankConditions(new List<InterestDependingFromBalance>(), fixedInterestOnBalance2,
                fixedCommission2, limitForSuspiciousAccounts2);
            string bankName2 = "Tinkoff";
            _centralBank.AddNewBank(conditions2, bankName2);
            _bank2 = _centralBank.GetBank(bankName2);

            _client1 = new RegularBankClient("Vasya", "Pupkin");
            _client2 = new RegularBankClient("Ivan", "Semenov");

            _debitAccount = new DebitAccount(_client1, _centralBank.GetIdForBankAccount());
            _depositAccount = new DepositAccount(_client1, _centralBank.GetIdForBankAccount(),
                new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day));

            double limit = 20000;
            _creditAccount = new CreditAccount(_client2, _centralBank.GetIdForBankAccount(), limit);

            _bank1.AddAccount(_debitAccount);
            _bank1.AddAccount(_creditAccount);
            _bank2.AddAccount(_depositAccount);
        }

        [Test]
        public void AddNewBank_NameHasBeenUsedAlready_ThrowException()
        {
            Assert.Catch<BanksException>(() =>
            {
                _centralBank.AddNewBank(_bank1.GetBankConditions(), _bank1.GetName());
            });
        }

        [Test]
        public void WithdrawMoneyFromAccount_PermissionDenied_ThrowException()
        {
            _bank1.TopUpABankAccount(_debitAccount.GetId(), 200);
            Assert.Catch<BanksException>(() =>
            {
                _bank1.WithdrawMoneyFromAccount(_debitAccount.GetId(), 5, _client2);
            });
        }

        [Test]
        public void WithdrawMoneyFromAccount_NotEnoughMoney_ThrowException()
        {
            _bank1.TopUpABankAccount(_debitAccount.GetId(), 200);
            Assert.Catch<BanksException>(() =>
            {
                _bank1.WithdrawMoneyFromAccount(_debitAccount.GetId(), 5000, _debitAccount.GetClient());
            });
        }

        [Test]
        public void WithdrawMoneyFromDepositAccount_TermDoesntFinished_ThrowException()
        {
            _bank2.TopUpABankAccount(_depositAccount.GetId(), 200);
            Assert.Catch<BanksException>(() =>
            {
                _bank2.WithdrawMoneyFromAccount(_depositAccount.GetId(), 5000, _depositAccount.GetClient());
            });
        }

        [Test]
        public void SubscribeClient_BankConditionsHasChanged_GetMessage()
        {
            _bank1.SubscribeAccountOwnerToGetMessages(_debitAccount.GetId());
            _bank1.ChangeFixedCommission(10000);
            CollectionAssert.IsNotEmpty(_debitAccount.GetClient().GetMessages());
        }

        [Test]
        public void WithdrawTooMuchMoneyFromSuspiciousAccount_ThrowException()
        {
            _bank1.TopUpABankAccount(_creditAccount.GetId(), 100000);
            Assert.Catch<BanksException>(() =>
            {
                _bank1.WithdrawMoneyFromAccount(_creditAccount.GetId(), 50000, _creditAccount.GetClient());
            });
        }

        [Test]
        public void TransferMoneyToAnotherAccount_SuccessfullyDone()
        {
            double balanceBefore1 = _creditAccount.GetBalance();
            double balanceBefore2 = _depositAccount.GetBalance();
            double transferValue = 5000;
            _bank1.TransferMoneyToAnotherAccount(_creditAccount.GetId(), _depositAccount.GetId(), transferValue, _creditAccount.GetClient());
            Assert.AreEqual(balanceBefore1 - transferValue, _creditAccount.GetBalance());
            Assert.AreEqual(balanceBefore2 + transferValue, _depositAccount.GetBalance());
        }

        [Test]
        public void CancelTransaction_SuccessfullyDone()
        {
            double balanceBefore1 = _creditAccount.GetBalance();
            double balanceBefore2 = _debitAccount.GetBalance();
            double transferValue = 5000;
            _bank1.TransferMoneyToAnotherAccount(_creditAccount.GetId(), _debitAccount.GetId(), transferValue, _creditAccount.GetClient());
            int transactionId = 1;
            _bank1.CancelTransaction(transactionId);
            Assert.AreEqual(balanceBefore1, _creditAccount.GetBalance());
            Assert.AreEqual(balanceBefore2, _depositAccount.GetBalance());
        }

        [Test]
        public void ClientAddAddress_AddressHadAddedBefore_ThrowException()
        {
            _client1.AddAddress(new Address("city", "street", "house"));
            Assert.Catch<BanksException>(() =>
            {
                _client1.AddAddress(new Address("city", "street", "house"));
            });
        }

        [Test]
        public void ClientAddAddressAndPassportData_BecomeToBeNotSuspicious()
        {
            _client1.AddAddress(new Address("city", "street", "house"));
            _client1.AddPassportData(new PassportData("0000", "000000"));
            Assert.IsFalse(_client1.IsSuspicious());
        }
    }
}