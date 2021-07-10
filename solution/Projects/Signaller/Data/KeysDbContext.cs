using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Signaller.Data
{
    public class KeysDbContext : DbContext, IDataProtectionKeyContext
    {
        public KeysDbContext(DbContextOptions<KeysDbContext> options)
            : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}