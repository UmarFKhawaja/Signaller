using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Signaller.Models
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public int Id { get; set; }

        // public string LoginProvider { get; set; }

        // public string ProviderKey { get; set; }

        // public string ProviderDisplayName { get; set; }

        // public string UserId { get; set; }

        public User User { get; set; }
    }
}