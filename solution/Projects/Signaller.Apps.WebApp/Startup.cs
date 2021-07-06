using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Signaller.Apps.WebApp.Data;

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
            try
            {
                dataContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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
