using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contract
{
    public interface ITokenService
    {
        string CreateToken(string userId, string userName, string email, IReadOnlyList<string> roules);
    }
}
