namespace Domain.Interfaces
{
    interface IBalance
    {
        decimal Value { get; }

        bool FromCache { get; }
    }
}
