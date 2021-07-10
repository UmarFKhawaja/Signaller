using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Signaller.Models
{
    public class UserClaim : IdentityUserClaim<int>
    {
        // public int Id { get; set; }

        // public string UserId { get; set; }

        // public string ClaimType { get; set; }

        // public string ClaimValue { get; set; }

        public User User { get; set; }
    }
}