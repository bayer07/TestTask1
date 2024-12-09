namespace WebAPI.Models
{
    public class CreditTransactionModel
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public override string ToString() => $"Id:{Id};ClientId:{ClientId};DateTime:{DateTime};Amount:{Amount}";
    }
}
