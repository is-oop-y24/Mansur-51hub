using Banks.Services.Accounts;
using Banks.Tools;

namespace Banks.Services.TransactionOperations
{
    public class TopUpABankAccountOperation : ITransactionOperation
    {
        private readonly double _moneyValue;
        private readonly IBankAccount _bankAccount;
        private readonly int _transactionId;

        public TopUpABankAccountOperation(IBankAccount bankAccount, double moneyValue, int transactionId)
        {
            _bankAccount = bankAccount;
            if (double.IsNegative(moneyValue))
            {
                throw new BanksException("Can not top up a negative value of money");
            }

            _moneyValue = moneyValue;
            _transactionId = transactionId;
        }

        public int GetTransactionId()
        {
            return _transactionId;
        }

        public void Execute()
        {
            if (_bankAccount.CouldBeNegativeBalance() && _bankAccount.GetBalance() < 0)
            {
                _bankAccount.IncrementANumberOfNegativeBalanceUsedTimes();
            }

            _bankAccount.SetBalance(_bankAccount.GetBalance() + _moneyValue);
            _bankAccount.AddTransaction(this);
        }

        public void Undo()
        {
            if (_bankAccount.GetBalance().CompareTo(_moneyValue) < 0 && !_bankAccount.CouldBeNegativeBalance())
            {
                throw new BanksException("Can not cancel transaction because not enough money");
            }

            _bankAccount.SetBalance(_bankAccount.GetBalance() - _moneyValue);
            _bankAccount.CancelTransaction(this);
        }
    }
}