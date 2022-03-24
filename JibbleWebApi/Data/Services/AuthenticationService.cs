using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Data.Services
{
    public interface IAuthenticationService
    {
        string GetAccessToken(int userId);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOptions<AuthenticationOptions> _options;

        public AuthenticationService(IOptions<AuthenticationOptions> options)
        {
            _options = options;
        }

        public string GetAccessToken(int userId)
        {
            var identity = GetIdentity(userId);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: identity.Claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(_options.Value.Lifetime)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Value.Secret)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(int id)
        {
            var claims = new List<Claim>
            {
                new("ID", id.ToString()),
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, null);
            return claimsIdentity;
        }
    }
}