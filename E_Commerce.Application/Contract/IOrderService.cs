using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contract
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetOredrsForSpecificUserAsync(string email, CancellationToken ct = default);
        Task<Result<OrderToReturnDto>> GetOredrByIdForSpecificUserAsync(string email,Guid id, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodAsync(CancellationToken ct =  default);
    }
}
