using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static E_Commerce.Application.Services.PaymentService;

namespace E_Commerce.Infrastructure.Payment
{
    public class StripePaymentGetWay : IPaymentGetWay
    {
        private readonly PaymentIntentService _paymentIntentService = new();
        public StripePaymentGetWay(IOptions<PaymentGetWaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.Secretkey;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var intentOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                AllowedPaymentMethodTypes = ["card"]
            };
            var intent = await _paymentIntentService.CreateAsync(intentOptions);
            return new PaymentIntentResult(intent.ClientSecret, intent.Id);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken ct = default)
        {
            var intentOptions = new PaymentIntentUpdateOptions
            {
                Amount = (long)amount,
                
            };
            var intent = await _paymentIntentService.UpdateAsync(paymentIntentId, intentOptions, cancellationToken: ct);
            return new PaymentIntentResult(intent.ClientSecret, intent.Id);
        }
    }
   
}
