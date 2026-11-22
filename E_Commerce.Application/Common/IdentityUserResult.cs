using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class IdentityUserResult
    {
        public IdentityUserResult(string id, string displayName, string userName, string email)
        {
            Id = id;
            DisplayName = displayName;
            UserName = userName;
            Email = email;
        }

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
