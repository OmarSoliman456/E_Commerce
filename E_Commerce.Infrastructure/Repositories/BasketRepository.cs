using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private IDatabase database = connection.GetDatabase();
        public async Task<CustomerBasket?> AddOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null, CancellationToken ct = default)
        {
            var value = JsonSerializer.Serialize(basket);
            var data = await database.StringSetAsync(basket.Id, value, TimeToLive ?? TimeSpan.FromDays(7));
            return data ? basket : null;
        }

        public async Task<bool> DeleteBasketAsync(string Id, CancellationToken ct = default)
        {
            return await database.KeyDeleteAsync(Id);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string Id, CancellationToken ct = default)
        {
            var basket = await database.StringGetAsync(Id);
            if (basket.IsNullOrEmpty) return null;
            var value = JsonSerializer.Deserialize<CustomerBasket>(basket!);
            if (value == null) return null;
            return value;
        }
    }
}
