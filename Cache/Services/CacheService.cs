using Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Cache.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<decimal?> GetDecimal(string key)
        {
            byte[]? byteArray = await _distributedCache.GetAsync(key);
            if (byteArray is null)
                return null;

            using MemoryStream memoryStream = new(byteArray);
            using BinaryReader binaryReader = new(memoryStream);
            return binaryReader.ReadDecimal();
        }

        public async Task SetDecimal(string key, decimal amount)
        {
            using MemoryStream memoryStream = new();
            await using BinaryWriter binaryWriter = new(memoryStream);
            binaryWriter.Write(amount);
            await _distributedCache.SetAsync(key, memoryStream.ToArray());
        }
    }
}
