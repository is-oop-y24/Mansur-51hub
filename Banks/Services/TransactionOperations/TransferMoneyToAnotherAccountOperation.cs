using System;
using Banks.Services.Accounts;
using Banks.Services.Bank;
using Banks.Tools;

namespace Banks.Services.TransactionOperations
{
    public class TransferMoneyToAnotherAccountOperation : ITransactionOperation
    {
        private readonly WithdrawMoneyFromTheAccountOperation _withdrawMoneyOperation;
        private readonly TopUpABankAccountOperation _topUpAccountOperation;

        public TransferMoneyToAnotherAccountOperation(double moneyValue, IBankAccount bankAccountFrom, IBankAccount bankAccountTo, BankConditions bankConditions, int transactionFromId, int transactionToId)
        {
            _withdrawMoneyOperation = new WithdrawMoneyFromTheAccountOperation(bankAccountFrom, moneyValue, bankConditions, transactionFromId);
            _topUpAccountOperation = new TopUpABankAccountOperation(bankAccountTo, moneyValue, transactionToId);
        }

        public int GetTransactionId()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            _withdrawMoneyOperation.Execute();
            try
            {
                _topUpAccountOperation.Execute();
            }
            catch (Exception e)
            {
                _withdrawMoneyOperation.Undo();
                throw new BanksException(e.Message);
            }
        }

        public void Undo()
        {
            _withdrawMoneyOperation.Undo();
            try
            {
                _topUpAccountOperation.Undo();
            }
            catch (Exception e)
            {
                _withdrawMoneyOperation.Execute();
                throw new BanksException(e.Message);
            }
        }
    }
}