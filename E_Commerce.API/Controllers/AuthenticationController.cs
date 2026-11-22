using E_Commerce.Application.Contract;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController(IAuthenticationService authenticationService) : ApiBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login, CancellationToken ct = default)
        {
            var result = await authenticationService.LoginAsync(login, ct);
            return ToActionResult(result);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegiserDto regiser, CancellationToken ct = default)
        {
            var result = await authenticationService.RegisterAsync(regiser, ct);
            return ToActionResult(result);
        }
        [HttpGet("emailExists/{email}")]
        public async Task<ActionResult<bool>> CheckEmail(string email, CancellationToken ct = default)
        {
            var result = await authenticationService.CheckEmailExistAsync(email, ct);
            return ToActionResult(result);
        }
        [HttpGet("currenUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct = default)
        {
            var email =User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await authenticationService.GetCurrentUserAsync(email, ct);
            return ToActionResult(result);
        }
        [HttpGet("adress")]
        [Authorize]
        public async Task<ActionResult<AdressDto>> GetCurrentUserAdress(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await authenticationService.GetCurrentUserAdressAsync(email, ct);
            return ToActionResult(result);
        }
        [HttpPut("adress")]
        [Authorize]
        public async Task<ActionResult<AdressDto>> UpdateCurrentUserAdress(AdressDto dto, CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await authenticationService.UpdateCurrentUserAdressAsync(email, dto, ct);
            return ToActionResult(result);
        }
    }
}
