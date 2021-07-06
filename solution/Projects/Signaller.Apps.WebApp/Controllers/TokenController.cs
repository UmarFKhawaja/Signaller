using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
            var token = GetJwtToken(User);

            return Content(JwtSecurityTokenHandler.WriteToken(token));
        }
        
        private JwtSecurityToken GetJwtToken
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
                    new Claim(JwtRegisteredClaimNames.Sub,user.Identity.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }
                .Concat(additionalClaims ?? Array.Empty<Claim>())
                .ToArray();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JwtBearer:IssuerSigningKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // var tokenDescription = new SecurityTokenDescriptor
            // {
            //     Issuer = issuer,
            //     Audience = audience,
            //     Expires = expires,
            //     Claims = claims,
            //     SigningCredentials = signingCredentials
            // };

            return new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                expires: expires,
                claims: claims,
                signingCredentials: signingCredentials
            );
        }
    }
}