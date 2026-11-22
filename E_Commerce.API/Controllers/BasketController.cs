using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.BasketDtos;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class BasketController(IBasketService service) : ApiBaseController
    {
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket, CancellationToken ct)
        {
            var value = await service.AddOrUpdateBasketAsync(basket, null, ct);
            return ToActionResult(value);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken ct)
        {
            var basket = await service.GetBasketAsync(id, ct);
            return ToActionResult(basket);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletBasket(string id, CancellationToken ct)
        {
            var basket = await service.DeleteBasketAsync(id, ct);
            return ToActionResult(basket);
        }
    }
}
