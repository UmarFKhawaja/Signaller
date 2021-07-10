using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Signaller.Data;
using Signaller.Models;

namespace Signaller.Apps.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationDbContext>
                (
                    (options) => options.UseMySQL(Configuration.GetConnectionString("Main"))
                );

            services
                .AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>
                (
                    (options) =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireNonAlphanumeric = false;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .AddIdentityServer()
                .AddApiAuthorization<User, PersistedGrantDbContext>();

            services
                .AddAuthentication()
                .AddIdentityServerJwt();

            services
                .AddControllers();

            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();
            
            services
                .Configure<ForwardedHeadersOptions>
                (
                    (options) =>
                    {
                        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;

                        if (!Environment.IsDevelopment())
                        {
                            var knownNetworks = Configuration["ForwardedHeadersOptions:KnownNetworks"];

                            if (!string.IsNullOrEmpty(knownNetworks))
                            {
                                foreach (var knownNetwork in knownNetworks.Split(";"))
                                {
                                    var parts = knownNetwork.Split(":");

                                    var prefix = parts[0];
                                    var prefixLength = int.Parse(parts[1]);

                                    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse(prefix), prefixLength));
                                }
                            }
                        }
                    }
                );
        }

        public void Configure(IApplicationBuilder app, ApplicationDbContext dataContext)
        {
            app.UseForwardedHeaders();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints
            (
                (endpoints) =>
                {
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                }
            );
        }
    }
}
