using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Signaller.Data;

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
                .AddDefaultIdentity<IdentityUser>
                (
                    (options) =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireNonAlphanumeric = false;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .AddIdentityServer
                (
                    (options) =>
                    {
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;

                        options.EmitStaticAudienceClaim = true;
                    }
                )
                .AddConfigurationStore
                (
                    (options) =>
                    { 
                        options.ConfigureDbContext = (builder) => builder.UseMySQL(Configuration.GetConnectionString("Main")); 
                    }
                )
                .AddOperationalStore
                (
                    (options) =>
                    { 
                        options.ConfigureDbContext = (builder) => builder.UseMySQL(Configuration.GetConnectionString("Main")); 
         
                        options.EnableTokenCleanup = true; 
                        options.TokenCleanupInterval = 30; 
                    }
                )
                .AddAspNetIdentity<IdentityUser>()
                .AddSigningCredential
                (
                    new X509Certificate2
                    (
                        File.ReadAllBytes(Configuration["IdentityServer:Key:FilePath"]),
                        (string)Configuration["IdentityServer:Key:Password"]
                    )
                // )
                // .AddServices
                // (
                //     Environment.IsDevelopment(),
                //     (builder) => builder.AddDeveloperSigningCredential(false),
                //     (builder) => builder.AddSigningCredential
                //     (
                //         new X509Certificate2
                //         (
                //             File.ReadAllBytes(Configuration["IdentityServer:Key:FilePath"]),
                //             (string)Configuration["IdentityServer:Key:Password"]
                //         )
                //     )
                );

            services
                .AddAuthentication();

            services
                .AddSingleton<JwtSecurityTokenHandler>();

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

    public static class StartupExtensions
    {
        public static IIdentityServerBuilder AddServices
        (
            this IIdentityServerBuilder identityServerBuilder,
            bool condition,
            Func<IIdentityServerBuilder, IIdentityServerBuilder> ifTrue,
            Func<IIdentityServerBuilder, IIdentityServerBuilder> ifFalse
        )
        {
            if (condition)
            {
                return ifTrue(identityServerBuilder);
            }
            else
            {
                return ifFalse(identityServerBuilder);
            }
        }
    }
}
