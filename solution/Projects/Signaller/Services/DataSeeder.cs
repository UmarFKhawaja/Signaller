using System;
using System.Linq;
using System.Threading.Tasks;
using Signaller.Contracts;
using Signaller.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Signaller.Services
{
    public class DataSeeder : IDataSeeder
    {
        public DataSeeder
        (
            ConfigurationDbContext configurationDbContext,
            PersistedGrantDbContext persistedGrantDbContext,
            KeysDbContext keysDbContext,
            ApplicationDbContext applicationDbContext
        )
            : base()
        {
            ConfigurationDbContext = configurationDbContext;
            PersistedGrantDbContext = persistedGrantDbContext;
            KeysDbContext = keysDbContext;
            ApplicationDbContext = applicationDbContext;
        }

        private ConfigurationDbContext ConfigurationDbContext { get; }

        private PersistedGrantDbContext PersistedGrantDbContext { get; }

        private KeysDbContext KeysDbContext { get; }

        private ApplicationDbContext ApplicationDbContext { get; }

        public async Task SeedDataAsync(IConfiguration configuration)
        {
            await MigrateConfigurationDbAsync();
            await MigratePersistedGrantDbAsync();
            await MigrateKeysDbAsync();
            await MigrateApplicationDbAsync();
            
            await SeedConfigurationDbAsync(configuration);
        }

        private async Task MigrateConfigurationDbAsync()
        {
            var context = ConfigurationDbContext;

            await context.Database.MigrateAsync();
        }

        private async Task MigratePersistedGrantDbAsync()
        {
            var context = PersistedGrantDbContext;

            await context.Database.MigrateAsync();
        }

        private async Task MigrateKeysDbAsync()
        {
            var context = KeysDbContext;
            
            await context.Database.MigrateAsync();
        }

        private async Task MigrateApplicationDbAsync()
        {
            var context = ApplicationDbContext;
            
            await context.Database.MigrateAsync();
        }

        private async Task SeedConfigurationDbAsync(IConfiguration configuration)
        {
            var context = ConfigurationDbContext;

            await context.EnsureSeedDataAsync(configuration);
        }
    }

    public static class SeedDataExtensions
    {
        public static async Task EnsureSeedDataAsync(this ConfigurationDbContext context, IConfiguration configuration)
        {
            if (!context.Clients.Any())
            {
                // TODO : log:Debug "Clients being populated"

                foreach (var client in Config.GetClients(configuration)().ToList())
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            else
            {
                // TODO : log:Debug "Clients already populated"
            }

            if (!context.IdentityResources.Any())
            {
                // TODO : log:Debug "IdentityResources being populated"

                foreach (var resource in Config.GetIdentityResources(configuration)().ToList())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            else
            {
                // TODO : log:Debug "IdentityResources already populated"
            }

            if (!context.ApiResources.Any())
            {
                // TODO : log:Debug "ApiResources being populated"

                foreach (var resource in Config.GetApiResources(configuration)().ToList())
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            else
            {
                // TODO : log:Debug "ApiResources already populated"
            }

            if (!context.ApiScopes.Any())
            {
                // TODO : log:Debug "ApiScopes being populated"

                foreach (var resource in Config.GetApiScopes(configuration)().ToList())
                {
                    await context.ApiScopes.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            else
            {
                // TODO : log:Debug "ApiScopes already populated"
            }
        }
    }
}