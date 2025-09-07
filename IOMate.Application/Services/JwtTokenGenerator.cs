using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace IOMate.Application.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimGroupRepository _claimGroupRepository;

        public JwtTokenGenerator(IConfiguration configuration, IClaimGroupRepository claimGroupRepository)
        {
            _configuration = configuration;
            _claimGroupRepository = claimGroupRepository;
        }

        public string GenerateToken(User user)
            => GenerateJwt(user, "access", DateTime.UtcNow.AddHours(2));

        public string GenerateRefreshToken(User user)
            => GenerateJwt(user, "refresh", DateTime.UtcNow.AddDays(7));

        private string GenerateJwt(User user, string typ, DateTime expires)
        {
            var claimGroups = _claimGroupRepository.GetUserClaimGroupsAsync(user.Id, CancellationToken.None)
                .GetAwaiter().GetResult();

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var claimsByResource = claimGroups
                .SelectMany(cg => cg.Claims)
                .GroupBy(c => c.Resource)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Action).ToArray());

            var userToSerialize = new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                Claims = claimsByResource
            };
            var userJson = JsonSerializer.Serialize(userToSerialize);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("typ", typ),
                new Claim("user", userJson)
            };

            if (typ == "access")
            {
                foreach (var resource in claimsByResource)
                {
                    foreach (var action in resource.Value)
                    {
                        claims.Add(new Claim($"{resource.Key}", action));
                    }
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool isRefreshToken = false)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                var typ = principal.FindFirst("typ")?.Value;
                if (isRefreshToken && typ != "refresh") return null;
                if (!isRefreshToken && typ != "access") return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}