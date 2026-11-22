using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.BasketDtos;
using E_Commerce.Application.DTOs.OrderDtos;
using E_Commerce.Application.Specification;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static E_Commerce.Application.Services.PaymentService;

namespace E_Commerce.Application.Services
{
    public class PaymentService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPaymentGetWay paymentGetWay,
        IOptions<PaymentGetWaySettings> options
        ) : IPaymentService
    {
        private readonly PaymentGetWaySettings _stripe = options.Value;
        
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(basketId, ct);
            if (basket is null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket.NotFound", $"Basket With Id{basketId} is not Found!"));
            if(!basket.Items.Any())
                return Result<BasketDto>.Fail(Error.NotFound("Basket.NotFound", $"Basket With Id{basketId} don't hase Items!"));
            foreach( var item in basket.Items)
            {
                var product = await unitOfWork.GetRepositoryAsync<Product, int>().GetByIdAsync(item.Id, ct);
                if (product == null)
                    return Result<BasketDto>.Fail(Error.NotFound("Product.NotFound", $"Product with id {item.Id} not found"));
                item.Price = product.Price;

            }
            var deliveryM = await unitOfWork.GetRepositoryAsync<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodeId.Value, ct);
            if( deliveryM is null )
                return Result<BasketDto>.Fail(Error.NotFound("DeliverMethode.NotFound", $"Delivery Methode is not found"));
            basket.ShippingPrice = deliveryM.Price;
            var subTotal = basket.Items.Sum(i => i.Quantity * i.Price);

            var amount = (long)Math.Round(subTotal + deliveryM.Price * 100m);

            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var result = await paymentGetWay.UpdatePaymentIntentAsync(basket.PaymentIntentId, amount, ct);
                basket.ClientSecret = result.ClientSecret;
                basket.PaymentIntentId = result.PaymentIntentId;
            }
            else
            {
                var result = await paymentGetWay.CreatePaymentIntentAsync(amount,_stripe.DefultCurrencr, ct);
                basket.ClientSecret = result.ClientSecret;
                basket.PaymentIntentId = result.PaymentIntentId;
            }
            var customerBasket = await basketRepository.AddOrUpdateBasketAsync(basket);
            return Result<BasketDto>.Ok(mapper.Map<BasketDto>(customerBasket));
        }
        public class PaymentGetWaySettings
        {
            public string Secretkey { get; set; } = default!;
            public string DefultCurrencr { get; set; } = "USD";
        }
    }
}
