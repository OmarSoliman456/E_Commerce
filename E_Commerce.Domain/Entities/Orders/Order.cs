using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    public class Order: BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, OrderAddress shippingAddress, ICollection<OrderItem> items, DeliveryMethod deliveryMethod, decimal subTotal)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            Items = items;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
        }

        [Required]
        public string UserEmail { get; set; } = default!;
        public OrderAddress ShippingAddress { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = [];
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatuse Status { get; set; } = OrderStatuse.pending;
        [ForeignKey("DeliveryMethod")]
        public int DeliveryMethodId { get; set; }

        public string? PaymentIntentId { get; set; }
        public decimal Total => SubTotal + (DeliveryMethod?.Price ?? 0);

    }
}
