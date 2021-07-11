using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Signaller.Data
{
    public class KeyDbContext : DbContext, IDataProtectionKeyContext
    {
        public KeyDbContext(DbContextOptions<KeyDbContext> options)
            : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}