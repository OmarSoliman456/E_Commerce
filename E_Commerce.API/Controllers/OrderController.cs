using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    public class OrderController(IOrderService orderService) : ApiBaseController
    {
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto dto, CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result =await orderService.CreateOrderAsync(dto, email, ct);
            return ToActionResult(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrders(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await orderService.GetOredrsForSpecificUserAsync(email, ct);
            return ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id, CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await orderService.GetOredrByIdForSpecificUserAsync(email,id, ct);
            return ToActionResult(result);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethods(CancellationToken ct = default)
        {
            var result = await orderService.GetAllDeliveryMethodAsync(ct);
            return ToActionResult(result);
        }

    }
}
