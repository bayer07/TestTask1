namespace WebAPI.Models
{
    public class RevertTransactionModel
    {
        public decimal ClientBalance { get; set; }

        public DateTime RevertDateTime { get; set; }
    }
}
