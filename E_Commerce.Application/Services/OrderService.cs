using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.OrderDtos;
using E_Commerce.Application.Specification;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class OrderService(
        IMapper mapper,
        IBasketService basketService,
        IUnitOfWork unitOfWork
        ) : IOrderService
    {
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            var basketResult = await basketService.GetBasketAsync(orderDto.BasketId, ct);
            if (!basketResult.IsSuccess)
                return Result<OrderToReturnDto>.Fail(basketResult.Errors);

            var basket = basketResult.Value;
            if (basket.Items.Count == 0)
                return Result<OrderToReturnDto>.Fail(Error.Validation("Basket.Empty", $"Basket with id {orderDto.BasketId} has no items"));

            var deliveryMethod = await unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetByIdAsync(orderDto.DelivryMethodId, ct);
            if (deliveryMethod == null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Delivery.NotFound", $"Delivery method {orderDto.DelivryMethodId} not found"));

            var productRepo = unitOfWork.GetRepositoryAsync<Product, int>();
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id, ct);
                if (product == null)
                    return Result<OrderToReturnDto>.Fail(Error.NotFound("Product.NotFound", $"Product with id {item.Id} not found"));

                var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                orderItems.Add(new OrderItem(productItemOrder, product.Price, item.Quantity));
            }

            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);
            var orderAddress = mapper.Map<OrderAddress>(orderDto.ShippingAddress);

            var order = new Order(email, orderAddress, orderItems, deliveryMethod, subTotal)
            {
                PaymentIntentId = basket.PaymentIntentId,
            };
            unitOfWork.GetRepositoryAsync<Order, Guid>().add(order);

            if (await unitOfWork.SaveChangesAsync(ct) <= 0)
                return Result<OrderToReturnDto>.Fail(Error.Failure("Order.CreateFailed", "Failed to create the order"));

            await basketService.DeleteBasketAsync(orderDto.BasketId, ct);

            return Result<OrderToReturnDto>.Ok(mapper.Map<OrderToReturnDto>(order));
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodAsync(CancellationToken ct = default)
        {
            var deliveryM = await unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetAllAsync(ct);
            if (deliveryM.Any())
            {
                return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryM));
            }
            return Result<IReadOnlyList<DeliveryMethodDto>>.Fail(Error.NotFound("DeliveryMethod.NotFound"));
        }

        public async Task<Result<OrderToReturnDto>> GetOredrByIdForSpecificUserAsync(string email,Guid id, CancellationToken ct = default)
        {
            var order = await unitOfWork.GetRepositoryAsync<Order, Guid>().GetByIdAsync(id, new OrderSpicification(email, id), ct);
            if (order is null)
            {
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Order.NotFound", $"Orders with id {id} is not found"));

            }
            return Result<OrderToReturnDto>.Ok(mapper.Map<OrderToReturnDto>(order));
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetOredrsForSpecificUserAsync(string email, CancellationToken ct = default)
        {
            var orders = await unitOfWork.GetRepositoryAsync<Order, Guid>().GetAllAsync(new OrderSpicification(email), ct);
            if (orders.Any())
            {
                return Result<IReadOnlyList<OrderToReturnDto>>.Ok(mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
            }
            return Result<IReadOnlyList<OrderToReturnDto>>.Fail(Error.NotFound("Orders.NotFound", $"Orders with email {email} is not found"));
        }
    }
}
