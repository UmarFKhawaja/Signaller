using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Signaller.Apps.WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext, IPersistedGrantDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync(CancellationToken.None);
        }

        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }

        public virtual DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
    }
}
