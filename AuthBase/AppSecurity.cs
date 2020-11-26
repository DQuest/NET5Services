using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AuthBase.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AuthBase
{
    public class AppSecurity
    {
        private readonly AuthOptions _authOptions;

        public AppSecurity(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions.Value ?? throw new ArgumentException(nameof(authOptions));
        }

        public string GetToken(
            string userId,
            DateTime from,
            IEnumerable<string> roles,
            IDictionary<string, string> additionalClaims = null)
        {
            var userIdentity = GetIdentity(userId, roles, additionalClaims);

            return GetJwtToken(from, userIdentity.Claims);
        }

        private ClaimsIdentity GetIdentity(
            string userId,
            IEnumerable<string> roles,
            IDictionary<string, string> additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            claims.AddRange(roles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims.Select(x => 
                    new Claim(x.Key, x.Value)));
            }

            return new ClaimsIdentity(claims, "Token");
        }

        private string GetJwtToken(DateTime from, IEnumerable<Claim> claims)
        {
            // Длительность активной сессии 12 часов, если нет иного
            var expires = from.Add(TimeSpan.FromMinutes(_authOptions.SessionLifeTime ?? 720));

            var authLoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Secret));

            var token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                expires: expires,
                claims: claims,
                signingCredentials: new SigningCredentials(authLoginKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}