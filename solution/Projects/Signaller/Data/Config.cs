using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;

namespace Signaller.Data
{
    public static class Config
    {
        public static readonly string[] IdentityScopes = new string[]
        {
            "openid",
            "profile",
            "email"
        };

        public static readonly string[] DataScopes = new string[]
        {
            "signaller.users",
            "signaller.conversations"
        };

        public static readonly string[] AllScopes = Array
            .Empty<string>()
            .Union(IdentityScopes)
            .Union(DataScopes)
            .ToArray();

        public static Func<IEnumerable<IdentityResource>> GetIdentityResources(IConfiguration configuration) =>
            () => new IdentityResource[]
            {
                new IdentityResources.OpenId
                {
                    DisplayName = "User identifier",
                    Description = "Your user identifier"
                },
                new IdentityResources.Profile
                {
                    DisplayName = "User profile",
                    Description = "Your user profile information (first name, last name, etc.)"
                },
                new IdentityResources.Email
                {
                    DisplayName = "User identifier",
                    Description = "Your user identifier"
                }
            };

        public static Func<IEnumerable<ApiResource>> GetApiResources(IConfiguration configuration) =>
            () => new ApiResource[]
            {
                new ApiResource
                {
                    Name = "signaller.api",
                    DisplayName = "Signaller API",
                    Description = "Allow clients to access the API",
                    Scopes = new List<string>(DataScopes),
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("893bfc0b-880c-4f5e-b258-41d007e08860".Sha256())
                    },
                    UserClaims = new List<string>
                    {
                    }
                }
            };

        public static Func<IEnumerable<ApiScope>> GetApiScopes(IConfiguration configuration) =>
            () => DataScopes.Select((dataScope) => new ApiScope(dataScope));

        public static Func<IEnumerable<Client>> GetClients(IConfiguration configuration) =>
            () => new Client[]
            {
                new Client
                {
                    ClientId = "signaller.api",
                    ClientName = "Signaller API",
                    ClientSecrets =
                    {
                        new Secret("893bfc0b-880c-4f5e-b258-41d007e08860".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = DataScopes
                },
                new Client
                {
                    ClientId = "signaller.web",
                    ClientName = "Signaller Web",
                    ClientSecrets =
                    {
                        new Secret("ca39181f-12ce-4a44-a4fd-0955b39c4953".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        $"{configuration["OIDC:Callback"]}/account/login-callback"
                    },
                    PostLogoutRedirectUris =
                    {
                        $"{configuration["OIDC:Callback"]}/account/logout-callback"
                    },
                    AllowedScopes = AllScopes,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false
                },
                new Client
                {
                    ClientId = "signaller.app",
                    ClientName = "Signaller App",
                    ClientSecrets =
                    {
                        new Secret("bd785003-0cce-4a77-9fec-516f033e3043".Sha256())
                    },
                    RedirectUris = { "urn:ietf:wg:oauth:2.0:oob" },
                    PostLogoutRedirectUris = { "https://notused" },
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = AllScopes,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                }
            };
    }
}