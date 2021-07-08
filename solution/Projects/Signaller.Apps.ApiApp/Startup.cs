using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Signaller.Apps.ApiApp.Hubs;
using Signaller.Apps.ApiApp.Types;

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
                .AddAuthentication
                (
                    (options) =>
                    {
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
                .AddJwtBearer
                (
                    JwtBearerDefaults.AuthenticationScheme,
                    (options) =>
                    {
                        options.Authority = Configuration["Authentication:JwtBearer:Authority"];
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Authentication:JwtBearer:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["Authentication:JwtBearer:Audience"]
                        };
                        options.RequireHttpsMetadata = false;
                    }
                );

            services
                .AddAuthorization();

            services
                .AddGraphQL
                (
                    (options) =>
                    {
                        options.EnableMetrics = false;
                    }
                )
                .AddNewtonsoftJson();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddSingleton<SignallerSchema>();
            services.AddSingleton<QueryType>();
            services.AddSingleton<MutationType>();
            services.AddSingleton<SubscriptionType>();
            services.AddSingleton<PostType>();
            services.AddSingleton<UserType>();
            
            services.Configure<KestrelServerOptions>
            (
                (options) =>
                {
                    options.AllowSynchronousIO = true;
                }
            );

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
                    endpoints.MapControllers();
                    endpoints.MapHub<ChatHub>("/chat");
                }
            );

            app.UseGraphQL<SignallerSchema>();

            app.UseGraphQLGraphiQL();
        }
    }
}
