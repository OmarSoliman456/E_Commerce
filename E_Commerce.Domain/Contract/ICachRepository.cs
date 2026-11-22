using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contract
{
    public interface ICachRepository
    {
        Task<string?>  GetAsync(string key, CancellationToken ct = default);
        Task SetAsync(string key, object value, TimeSpan? Duration = null, CancellationToken ct = default);
    }
}
