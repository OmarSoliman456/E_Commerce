using E_Commerce.Application.Contract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.IdentityService
{
    public class TokenService : ITokenService
    {
        public string CreateToken(string userId, string userName, string email, IReadOnlyList<string> roules)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.GivenName, userName),
                new Claim(ClaimTypes.Email, email),
            };
            foreach(var roule in roules)
            {
                claims.Add(new Claim(ClaimTypes.Role, roule));
            }
            var secrete = "HayIamoMarHayIamoMarHayIamoMarHayIamoMarHayIamoMarHayIamoMarHayIamoMar";
            var secretekey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrete));

            var jwtToken = new JwtSecurityToken(
                issuer: "https://localhost:7045",
                audience: "MyOnlineStore",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: new SigningCredentials(secretekey, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
