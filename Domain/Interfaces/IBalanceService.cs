using Domain.Transactions;

namespace Domain.Interfaces
{
    public interface IBalanceService
    {
        Task<ITransactionResult> CreditTransaction(ITransaction transaction);

        Task<ITransactionResult> DebitTransaction(ITransaction transaction);

        Task<ITransactionResult> RevertTransaction(Guid transactionId);

        Task<ITransactionResult> GetTransaction(Guid transactionId);
    }
}
