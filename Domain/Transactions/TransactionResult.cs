using Domain.Interfaces;

namespace Domain.Transactions
{
    public class TransactionResult : ITransactionResult
    {
        public DateTime DateTime { get; set; }

        public decimal Balance { get; set; }
    }
}
