using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.BasketDtos
{
    public class BasketItemDto
    {
        [Required(ErrorMessage = "Product Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name is required")]
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;

        [Range(0,double.MaxValue, ErrorMessage ="Price Must be Positive")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least more than one")]

        public int Quantity { get; set; }
    }
}
