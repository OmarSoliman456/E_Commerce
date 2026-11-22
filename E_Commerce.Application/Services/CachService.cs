using E_Commerce.Application.Contract;
using E_Commerce.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class CachService(ICachRepository repository) : ICachService
    {
        public async Task<string?> GetAsync(string key, CancellationToken ct = default)
        {
            return await repository.GetAsync(key, ct);
        }

        public async Task SetAsync(string key, object value, TimeSpan? Duration = null, CancellationToken ct = default)
        {
            await repository.SetAsync(key, value, Duration ?? TimeSpan.FromMinutes(1), ct);
        }
    }
}
