using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Signaller.Apps.ApiApp.Hubs;

namespace Signaller.Apps.ApiApp
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
            services.AddCors
            (
                (options) =>
                {
                    options.AddDefaultPolicy
                    (
                        (builder) =>
                        {
                            var corsOrigins = Configuration["Cors:Origins"] ?? string.Empty;

                            builder
                                .WithOrigins(corsOrigins.Split(";"))
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        }
                    );
                }
            );

            services.AddControllers();

            services
                .AddSignalR()
                .AddStackExchangeRedis
                (
                    Configuration.GetConnectionString("Cache"),
                    (options) =>
                    {
                        options.Configuration.ChannelPrefix = "Signaller";
                    }
                );

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer
                (
                    JwtBearerDefaults.AuthenticationScheme,
                    (options) =>
                    {
                        options.Authority = Configuration["Authentication:Bearer:Authority"];
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };
                        options.RequireHttpsMetadata = false;
                    }
                );

            services
                .AddAuthorization();
            
            services
                .Configure<ForwardedHeadersOptions>
                (
                    (options) =>
                    {
                        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                        if (!Environment.IsDevelopment())
                        {
                            var knownNetworks = Configuration["ForwardedHeadersOptions:KnownNetworks"].Split(";");

                            foreach (var knownNetwork in knownNetworks)
                            {
                                var parts = knownNetwork.Split(":");

                                var prefix = parts[0];
                                var prefixLength = int.Parse(parts[1]);

                                options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse(prefix), prefixLength));
                            }
                        }
                    }
                );
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints
            (
                (endpoints) =>
                {
                    endpoints.MapHub<ChatHub>("/chat");
                }
            );
        }
    }
}
