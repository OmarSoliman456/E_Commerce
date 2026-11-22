using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contract
{
    public interface IAuthenticationService
    {
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct  = default);
        Task<Result<UserDto>> RegisterAsync(RegiserDto regiserDto, CancellationToken ct  = default);
        Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default);
        Task<Result<AdressDto>> GetCurrentUserAdressAsync(string email, CancellationToken ct = default);
        Task<Result<AdressDto>> UpdateCurrentUserAdressAsync(string email, AdressDto dto, CancellationToken ct = default);


    }
}
