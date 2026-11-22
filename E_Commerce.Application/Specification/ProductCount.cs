using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class ProductCount : BaseSpicification<Product, int>
    {
        public ProductCount(ProductQueryParam queryParam) :
            base
            (
                p =>
                (!queryParam.BrandId.HasValue || p.BrandId == queryParam.BrandId)
                &&
                (!queryParam.TypeId.HasValue || p.TypeId == queryParam.TypeId)
                &&
                (string.IsNullOrEmpty(queryParam.SearchValue) || p.Name.ToLower().Contains(queryParam.SearchValue))
            )
        {
            
        }
    }
}
