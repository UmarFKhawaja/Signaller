using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Signaller.Apps.WebApp.Controllers
{
    [Route("token")]
    public class TokenController : Controller
    {
        public TokenController(JwtSecurityTokenHandler jwtSecurityTokenHandler, IConfiguration configuration)
            : base()
        {
            JwtSecurityTokenHandler = jwtSecurityTokenHandler;
            Configuration = configuration;
        }
        
        private JwtSecurityTokenHandler JwtSecurityTokenHandler { get; }

        private IConfiguration Configuration { get; }

        [Authorize]
        [Route("")]
        public IActionResult Index()
        {
            var securityTokenDescriptor = GetSecurityTokenDescriptor(User);
            var token = JwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var content = JwtSecurityTokenHandler.WriteToken(token);

            return Content(content);
        }
        
        private SecurityTokenDescriptor GetSecurityTokenDescriptor
        (
            ClaimsPrincipal user,
            Claim[] additionalClaims = null
        )
        {
            var issuer = Configuration["Authentication:JwtBearer:Issuer"];
            var audience = Configuration["Authentication:JwtBearer:Audience"];

            if (!int.TryParse(Configuration["Authentication:JwtBearer:Expiration"], out var expiration))
            {
                expiration = 15;
            }

            var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(expiration));
            var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Identity.Name, ClaimValueTypes.String, issuer, issuer),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, issuer, issuer)
                }
                .Concat(additionalClaims ?? Array.Empty<Claim>())
                .ToDictionary((claim) => claim.Type, (claim) => (object) claim);

            var certificate = new X509Certificate2
            (
                System.IO.File.ReadAllBytes(Configuration["IdentityServer:Key:FilePath"]),
                (string) Configuration["IdentityServer:Key:Password"]
            );
            var signingCredentials = new X509SigningCredentials(certificate);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Expires = expires,
                Claims = claims,
                SigningCredentials = signingCredentials
            };

            return securityTokenDescriptor;
        }
    }
}