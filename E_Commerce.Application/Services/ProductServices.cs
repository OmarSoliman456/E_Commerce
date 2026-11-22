using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.Specification;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class ProductService(IUnitOfWork unitofwork, IMapper mapper) : IProductService
    {
        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct)
        {
            var brands = await unitofwork.GetRepositoryAsync<ProductBrand, int>().GetAllAsync(ct);
            var brandsdto = mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(brandsdto);
        }

        public async Task<Result<PaginationResult<ProductDto>>> GetAllProductsAsync(ProductQueryParam queryParam, CancellationToken ct)
        {

            var spec = new ProductsWithBrandAndType(queryParam);

            var cot = new ProductCount(queryParam);

            var products = await unitofwork.GetRepositoryAsync<Product, int>().GetAllAsync(spec, ct);

            var count = await unitofwork.GetRepositoryAsync<Product, int>().CountAsync(cot, ct);

            var productsdto = mapper.Map<IReadOnlyList<ProductDto>>(products);

            var value = new PaginationResult<ProductDto>(queryParam.PageIndex, queryParam.PageSize, count, productsdto);

            return Result<PaginationResult<ProductDto>>.Ok(value);
        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct)
        {
            var types = await unitofwork.GetRepositoryAsync<ProductType, int>().GetAllAsync(ct);
            var typedto = mapper.Map<IReadOnlyList<TypeDto>>(types);
            return Result<IReadOnlyList<TypeDto>>.Ok(typedto);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct)
        {
            var spec = new ProductsWithBrandAndType(id);
            spec.AddInclude(p => p.ProductType);
            var product = await unitofwork.GetRepositoryAsync<Product, int>().GetByIdAsync(id,spec, ct);
            if (product == null)
            {
                return Result<ProductDto>.Fail(Error.NotFound("Product.Not Found", "Not Found"));
            }
            var productdto = mapper.Map<ProductDto>(product);
            return Result<ProductDto>.Ok(productdto);
        }
    }
}

