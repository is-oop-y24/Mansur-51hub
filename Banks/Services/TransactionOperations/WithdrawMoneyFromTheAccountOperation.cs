using Banks.Services.Accounts;
using Banks.Services.Bank;
using Banks.Tools;

namespace Banks.Services.TransactionOperations
{
    public class WithdrawMoneyFromTheAccountOperation : ITransactionOperation
    {
        private readonly double _moneyValue;
        private readonly IBankAccount _bankAccount;
        private readonly BankConditions _bankConditions;
        private readonly int _transactionId;

        public WithdrawMoneyFromTheAccountOperation(IBankAccount bankAccount, double moneyValue, BankConditions bankConditions, int transactionId)
        {
            _bankAccount = bankAccount;

            if (double.IsNegative(moneyValue))
            {
                throw new BanksException("Could not withdraw a negative value of money");
            }

            _moneyValue = moneyValue;
            _bankConditions = bankConditions;
            _transactionId = transactionId;
        }

        public int GetTransactionId()
        {
            return _transactionId;
        }

        public void Execute()
        {
            double currentBalance = _bankAccount.GetBalance();
            if (currentBalance.CompareTo(_moneyValue) < 0 && !_bankAccount.CouldBeNegativeBalance())
            {
                throw new BanksException("Not enough money to withdraw");
            }

            if (currentBalance.CompareTo(_bankConditions.LimitForSuspiciousAccounts) > 0 &&
                _bankAccount.IsTheAccountSuspicious())
            {
                throw new BanksException("Suspicious account trying withdraw too much money");
            }

            if (!_bankAccount.CanWithdrawOrTransferMoney())
            {
                throw new BanksException("Term has not expired yet");
            }

            if (_bankAccount.CouldBeNegativeBalance() && _bankAccount.GetBalance() < 0)
            {
                _bankAccount.IncrementANumberOfNegativeBalanceUsedTimes();
            }

            _bankAccount.SetBalance(currentBalance - _moneyValue);
            _bankAccount.AddTransaction(this);
        }

        public void Undo()
        {
            _bankAccount.SetBalance(_bankAccount.GetBalance() + _moneyValue);
            _bankAccount.CancelTransaction(this);
        }
    }
}