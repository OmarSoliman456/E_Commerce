using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class AuthenticationService(IIdentityService identityService, ITokenService tokenService) : IAuthenticationService
    {
        public async Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default)
        {
            var result = await identityService.FindUserByEmailAsync(email, ct);
            if (result.IsSuccess)
                return Result<bool>.Ok(true);
            return Result<bool>.Fail(result.Errors);
        }

        public async Task<Result<AdressDto>> GetCurrentUserAdressAsync(string email, CancellationToken ct = default)
        {
            return await identityService.GetCurrentUserAdressAsync(email, ct);
        }

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var result = await identityService.FindUserByEmailAsync(email, ct);
            if (result.IsSuccess)
            {
                var user = result.Value;
                var roles = await identityService.GetUserRolesAsync(user.Email, ct);
                var token = tokenService.CreateToken(user.Id, user.UserName, user.Email, roles.Value);
                return Result<UserDto>.Ok(new UserDto()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = token,
                });
            }
            return Result<UserDto>.Fail(result.Errors);
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            var user = await identityService.FindUserByEmailAsync(loginDto.Email);
            if (!user.IsSuccess)
                return Result<UserDto>.Fail(Error.NotFound());

            var pass = await identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password);
            if(!pass.IsSuccess)
                return Result<UserDto>.Fail(pass.Errors);
            var roles = await identityService.GetUserRolesAsync(loginDto.Email);
            var token = tokenService.CreateToken(user.Value.Id, user.Value.UserName, user.Value.Email, roles.Value);


            return Result<UserDto>.Ok(new UserDto()
            {
                Email = user.Value.Email,
                Token = token,
                DisplayName = user.Value.DisplayName

            });
        }

        public async Task<Result<UserDto>> RegisterAsync(RegiserDto regiserDto, CancellationToken ct = default)
        {

            var Createuser = await identityService.CreateUserAsync(regiserDto, ct);
            if(!Createuser.IsSuccess)
                return Result<UserDto>.Fail(Createuser.Errors);

            var roles = await identityService.GetUserRolesAsync(regiserDto.Email);
            var token = tokenService.CreateToken(Createuser.Value.Id, Createuser.Value.UserName, Createuser.Value.Email, roles.Value);
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = Createuser.Value.Email,
                DisplayName = Createuser.Value.DisplayName,
                Token = token
            });

        }

        public async Task<Result<AdressDto>> UpdateCurrentUserAdressAsync(string email, AdressDto dto, CancellationToken ct = default)
        {
            return await identityService.UpdateCurrentUserAdressAsync(email, dto, ct);
        }
    }
}
