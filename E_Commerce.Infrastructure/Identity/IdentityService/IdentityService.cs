using E_Commerce.Application.Common;
using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.IdentityService
{
    public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
    {
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<bool>.Fail(Error.NotFound());
            var checkPass = await userManager.CheckPasswordAsync(user, password);
            if(!checkPass)
                return Result<bool>.Fail(Error.InvalidCredentials());

            return Result<bool>.Ok(true);

        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegiserDto regiserDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = regiserDto.Email,
                DisplayName = regiserDto.DisplayName,
                UserName = regiserDto.UserName,
                PhoneNumber = regiserDto.PhoneNumber
            };
            var Createuser = await userManager.CreateAsync(user, regiserDto.Password);
            if (!Createuser.Succeeded)
            {
                var errors = Createuser.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.UserName, user.Email));

        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IdentityUserResult>.Fail(Error.NotFound());
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.UserName, user.Email));

        }

        public async Task<Result<AdressDto>> GetCurrentUserAdressAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(a => a.Adress).Where(a => a.Email == email).FirstOrDefaultAsync();
            if(user == null)
                return Result<AdressDto>.Fail(Error.NotFound());
            if(user.Adress is null)
                return Result<AdressDto>.Fail(Error.NotFound($"User with email{email} not has an adress"));

            return Result<AdressDto>.Ok(new AdressDto()
            {
                City = user.Adress.City,
                Country = user.Adress.Country,
                Street = user.Adress.Street,
                FirstName = user.Adress.FirstName,
                LastName = user.Adress.LastName
            });

        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound());
            var roles = await userManager.GetRolesAsync(user);
            return Result<IReadOnlyList<string>>.Ok(roles.ToList());

        }

        public async Task<Result<AdressDto>> UpdateCurrentUserAdressAsync(string email, AdressDto dto, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(a => a.Adress).Where(a => a.Email == email).FirstOrDefaultAsync();
            if (user == null)
                return Result<AdressDto>.Fail(Error.NotFound());
            if (user.Adress is null)
            {
                user.Adress = new Adress()
                {
                    City = dto.City,
                    Street = dto.Street,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Country = dto.Country,
                };
            }
            else
            {
                user.Adress.FirstName = dto.FirstName;
                user.Adress.LastName = dto.LastName;
                user.Adress.Country = dto.Country;
                user.Adress.Street = dto.Street;
                user.Adress.Country = dto.Country;
            }
            var result = await userManager.UpdateAsync(user);
            if(!result.Succeeded)
                return Result<AdressDto>.Fail(Error.InvalidCredentials());

            return Result<AdressDto>.Ok(dto);

        }
    }
}
