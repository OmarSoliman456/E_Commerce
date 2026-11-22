using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.BasketDtos;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class BasketService(IBasketRepository repository, IMapper mapper) : IBasketService
    {
        public async Task<Result<BasketDto>> AddOrUpdateBasketAsync(BasketDto basketdto, TimeSpan? TimeToLive = null, CancellationToken ct = default)
        {
            var basket = mapper.Map<CustomerBasket>(basketdto);

            var data = await repository.AddOrUpdateBasketAsync(basket);
            if (data is null)
                return Result<BasketDto>.Fail(Error.Failure("Basket.CreateOrUpdate.Failure", "Failed to Create Or Update Basket"));
            return Result<BasketDto>.Ok(basketdto);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string Id, CancellationToken ct = default)
        {
            var basket = await repository.GetBasketAsync(Id);
            if (basket is null)
                return Result<bool>.Fail(Error.NotFound("Basket.Delete.NotFound", "Failed to Delete Basket"));

            var value = await repository.DeleteBasketAsync(Id);
            return value ? Result<bool>.Ok(value) : Result<bool>.Fail(Error.Failure("Basket.Delete.Failure", "Failed to Delete Basket"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string Id, CancellationToken ct = default)
        {
            var basket = await repository.GetBasketAsync(Id);
            if (basket is null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket.Get.NotFound", "Failed to Get Basket"));
            var basketdto = mapper.Map<BasketDto>(basket);
            return Result<BasketDto>.Ok(basketdto);
        }
    }
}
