using System.Collections.Generic;
using Banks.Services.TransactionOperations;

namespace Banks.Services.Bank
{
    public class Transactions : List<ITransactionOperation>
    {
    }
}