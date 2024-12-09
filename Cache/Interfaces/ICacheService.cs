namespace Cache.Interfaces
{
    public interface ICacheService
    {
        Task<decimal?> GetDecimal(string clientId);

        Task SetDecimal(string key, decimal amount);
    }
}
