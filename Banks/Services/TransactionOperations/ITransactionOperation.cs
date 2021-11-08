namespace Banks.Services.TransactionOperations
{
    public interface ITransactionOperation
    {
        int GetTransactionId();
        void Execute();
        void Undo();
    }
}