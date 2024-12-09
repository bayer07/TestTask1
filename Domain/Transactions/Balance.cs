using Domain.Interfaces;

namespace Domain.Transactions
{
    internal class Balance : IBalance
    {
        public decimal Value { get; set; }
        public bool FromCache { get; set; }
    }
}
