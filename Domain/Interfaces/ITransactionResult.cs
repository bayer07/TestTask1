namespace Domain.Interfaces
{
    public interface ITransactionResult
    {
        DateTime DateTime { get; }

        decimal Balance { get; }
    }
}