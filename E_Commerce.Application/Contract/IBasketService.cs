using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.BasketDtos;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contract
{
    public interface IBasketService
    {
        Task<Result<BasketDto>> AddOrUpdateBasketAsync(BasketDto basket, TimeSpan? TimeToLive = default, CancellationToken ct = default);
        Task<Result<BasketDto>> GetBasketAsync(string Id, CancellationToken ct = default);
        Task<Result<bool>> DeleteBasketAsync(string Id, CancellationToken ct = default);
    }
}
