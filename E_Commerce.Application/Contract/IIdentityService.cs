using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contract
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string email,string password, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegiserDto regiserDto, CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default);
        Task<Result<AdressDto>> GetCurrentUserAdressAsync(string email, CancellationToken ct = default);
        Task<Result<AdressDto>> UpdateCurrentUserAdressAsync(string email,AdressDto dto, CancellationToken ct = default);

    }
}
