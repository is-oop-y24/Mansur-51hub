namespace Banks.Services.CentralBank
{
    public class TransactionIdGenerator
    {
        private int _id;

        public TransactionIdGenerator()
        {
            _id = 0;
        }

        public int GenerateId()
        {
            _id++;
            return _id;
        }
    }
}