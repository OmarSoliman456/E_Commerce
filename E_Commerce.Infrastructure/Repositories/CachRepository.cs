using E_Commerce.Domain.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class CachRepository(IConnectionMultiplexer connection) : ICachRepository
    {
        private IDatabase database = connection.GetDatabase();
        public async Task<string?> GetAsync(string key, CancellationToken ct = default)
        {
            var value = await database.StringGetAsync(key);
            if(value.IsNullOrEmpty)
                return null;
            return value;
        }

        public async Task SetAsync(string key, object value, TimeSpan? Duration = null, CancellationToken ct = default)
        {
            var redisvalue = JsonSerializer.Serialize(value);
            var result = await database.StringSetAsync(key, redisvalue, Duration ?? TimeSpan.FromMinutes(1));
        }
    }
}
