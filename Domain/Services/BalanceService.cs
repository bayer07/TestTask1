using Cache.Interfaces;
using Data;
using Domain.Interfaces;
using Domain.Transactions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly ICacheService _cacheService;
        private readonly ApplicationContext _context;
        private readonly IValidator<DebitTransaction> _debitValidator;
        private readonly IValidator<CreditTransaction> _creditValidator;

        public BalanceService(
            ICacheService cacheService,
            ApplicationContext context,
            IValidator<DebitTransaction> debitValidator,
            IValidator<CreditTransaction> creditValidator)
        {
            _cacheService = cacheService;
            _context = context;
            _debitValidator = debitValidator;
            _creditValidator = creditValidator;
        }

        public async Task<ITransactionResult> CreditTransaction(ITransaction transaction)
        {
            CreditTransaction creditTransaction = (CreditTransaction)transaction;
            await _creditValidator.ValidateAndThrowAsync(creditTransaction);

            ITransactionResult transactionResult = await DoTransaction(creditTransaction);
            return transactionResult;
        }

        public async Task<ITransactionResult> DebitTransaction(ITransaction transaction)
        {
            var lastTransaction = await _context.Transactions
                .AsNoTracking()
                .Where(t => t.ClientId == transaction.ClientId)
                .OrderByDescending(t => t.DateTime)
                .FirstAsync();
            IBalance balance = await GetBalance(lastTransaction);
            DebitTransaction debitTransaction = (DebitTransaction)transaction;
            ValidationContext<DebitTransaction> context = new(debitTransaction)
            {
                RootContextData = { ["Balance"] = balance.Value }
            };
            await _debitValidator.ValidateAsync(context);

            ITransactionResult transactionResult = await DoTransaction(debitTransaction);
            return transactionResult;
        }

        public async Task<ITransactionResult> RevertTransaction(Guid transactionId)
        {
            Transaction transaction = await _context.Transactions
                .SingleAsync(t => t.Id == transactionId);
            transaction.RevertDateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            IBalance balance = await GetBalance(transaction);
            decimal balanceValue = balance.Value;
            if (balance.FromCache)
            {
                balanceValue = balance.Value - transaction.Amount;
                await SetBalance(transactionId, balanceValue);
            }

            ITransactionResult transactionResult = new TransactionResult { Balance = balanceValue, DateTime = (DateTime)transaction.RevertDateTime };
            return transactionResult;
        }

        public async Task<ITransactionResult> GetTransaction(Guid transactionId)
        {
            ITransaction transaction = await _context.Transactions.SingleAsync(t => t.Id == transactionId);
            IBalance balance = await GetBalance(transaction);
            return new TransactionResult { Balance = balance.Value, DateTime = transaction.DateTime };
        }

        private async Task<IBalance> GetBalance(ITransaction transaction)
        {
            string key = $"transaction:{transaction.Id.ToString()}:balance";
            decimal? balance = await _cacheService.GetDecimal(key);
            bool fromCache = true;
            if (balance == null)
            {
                fromCache = false;
                balance = _context.Transactions
                    .Where(t => t.ClientId == transaction.ClientId && transaction.DateTime >= t.DateTime && t.RevertDateTime == null)
                    .Sum(t => t.Amount);
                await _cacheService.SetDecimal(key, (decimal)balance);
            }

            return new Balance { FromCache = fromCache, Value = (decimal)balance };
        }

        private async Task SetBalance(Guid transactionId, decimal balance)
        {
            string key = $"transaction:{transactionId.ToString()}:balance";
            await _cacheService.SetDecimal(key, balance);
        }

        private async Task<ITransactionResult> DoTransaction(Transaction transaction)
        {
            bool isExists = await _context.Transactions
                .AsNoTracking()
                .AnyAsync(t => t.Id == transaction.Id);
            if (!isExists)
            {
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                IBalance balance = await GetBalance(transaction);
                decimal balanceValue = balance.FromCache ? balance.Value + transaction.Amount : balance.Value;
                await SetBalance(transaction.Id, balanceValue);
                return new TransactionResult { Balance = balanceValue, DateTime = transaction.DateTime };
            }

            return await GetTransaction(transaction.Id);
        }
    }
}
