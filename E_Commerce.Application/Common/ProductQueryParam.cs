using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class ProductQueryParam
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public String? SearchValue { get; set; }
        public ProductSort? SortValue { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
    public enum ProductSort
    {
        None = 0,
        NameAsc = 1,
        NameDesc = 2,
        PricaAsc = 3,
        PricaDesc = 4,
    }
}
