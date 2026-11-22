using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.OrderDtos
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; } = default!;

        [Required]
        public int DelivryMethodId { get; set; }


        [Required]
        public OrderAddress ShippingAddress { get; set; } = default!;
    }
}
