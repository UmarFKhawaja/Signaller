using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Signaller.Models
{
    public class UserRole : IdentityUserRole<string>
    {
        public string Id { get; set; }

        // public string UserId { get; set; }

        // public string RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}