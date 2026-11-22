using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contract
{
    public interface IProductService
    {
        Task<Result<PaginationResult<ProductDto>>> GetAllProductsAsync(ProductQueryParam queryParam, CancellationToken ct);
        Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct);
        Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct);
        Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct);
    }
}
