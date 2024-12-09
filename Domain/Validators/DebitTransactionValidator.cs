using Domain.Transactions;
using FluentValidation;

namespace Domain.Validators
{
    public class DebitTransactionValidator : AbstractValidator<DebitTransaction>
    {
        public DebitTransactionValidator()
        {
            RuleFor(transaction => transaction.DateTime)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Дата не может быть в будущем");

            RuleFor(transaction => transaction.Amount)
                .GreaterThan(0)
                .WithMessage("Сумма в транзакции должна быть всегда положительной");
            RuleFor(transaction => transaction.Amount).Custom((amount, context) =>
            {
                if (context.RootContextData.TryGetValue("Balance", out var value))
                {
                    decimal balance = (decimal)value;
                    if (balance < -amount)
                        throw new ValidationException("У клиента нельзя списать больше средств, чем есть на балансе");
                }
            });
        }
    }
}
