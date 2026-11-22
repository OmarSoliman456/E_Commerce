using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class ProductsWithBrandAndType : BaseSpicification<Product, int>
    {
        public ProductsWithBrandAndType(ProductQueryParam queryParam) :
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
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            switch(queryParam.SortValue)
            {
                case ProductSort.NameAsc:
                    AddOrderAsc(p => p.Name);
                    break;
                case ProductSort.NameDesc:
                    AddOrderDesc(p => p.Name);
                    break;
                case ProductSort.PricaAsc:
                    AddOrderAsc(p => p.Price);
                        break;
                case ProductSort.PricaDesc:
                    AddOrderDesc(p => p.Price);
                        break;
                default:
                    AddOrderAsc(p => p.Id);
                    break;
            }
            AddPagination(queryParam.PageIndex, queryParam.PageSize);
        }
        public ProductsWithBrandAndType(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
