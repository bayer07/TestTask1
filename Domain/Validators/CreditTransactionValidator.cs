using Data;
using Domain.Transactions;
using FluentValidation;

namespace Domain.Validators
{
    public class CreditTransactionValidator : AbstractValidator<CreditTransaction>
    {
        public CreditTransactionValidator()
        {
            RuleFor(transaction => transaction.DateTime)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Дата не может быть в будущем");

            RuleFor(transaction => transaction.Amount)
                .GreaterThan(0)
                .WithMessage("Сумма в транзакции должна быть всегда положительной");
        }
    }
}
