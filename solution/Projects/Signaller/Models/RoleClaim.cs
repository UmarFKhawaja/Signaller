using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Signaller.Models
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        // public int Id { get; set; }

        // public string RoleId { get; set; }

        // public string ClaimType { get; set; }

        // public string ClaimValue { get; set; }

        public Role Role { get; set; }
    }
}