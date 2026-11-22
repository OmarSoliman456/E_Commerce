using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.OrderDtos
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        [Required]
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; } = default!;
        public AdressDto ShippingAddress { get; set; } = default!;
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public string DeliveryMethod { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public decimal DeliveruCost { get; set; }
        public decimal Total { get; set; }





    }
}
