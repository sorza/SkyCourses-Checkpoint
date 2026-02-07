using Microsoft.IdentityModel.Tokens;
using Sky.Api.Application.Interfaces;
using Sky.Api.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Sky.Api.Infrastructure.Services
{
    public class TokenService(IConfiguration configuration, IRepository<RefreshToken> repository) : ITokenService
    {       

        public string GenerateToken(string userId, string email, IEnumerable<string> roles)
        {
            var jwtIssuer = configuration["Jwt:Issuer"]!;
            var jwtAudience = configuration["Jwt:Audience"]!;
            var jwtSecretKey = configuration["Jwt:SecretKey"]!;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public async Task<string?> ValidateRefreshToken(string refreshToken)
        {
            var storedToken = await repository.GetAsync(rt => rt.Token == refreshToken);

            if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                return null;

            return storedToken.UserId;
        }

        public async Task SaveRefreshToken(RefreshToken token, CancellationToken cancellationToken = default)
        {
            var existingToken = await repository.GetAsync(rt => rt.Token == token.Token, cancellationToken);

            if (existingToken is not null)
                throw new InvalidOperationException("Refresh token já existe.");

            await repository.CreateAsync(token, cancellationToken);
        }

        public async Task<RefreshToken?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken = default)
            => await repository.GetAsync(rt => rt.Token == refreshToken, cancellationToken);        

        public async Task UpdateRefreshToken(RefreshToken token, CancellationToken cancellationToken = default)
        {
            var existingToken = await repository.GetAsync(rt => rt.Token == token.Token, cancellationToken);

            if (existingToken is null)
                throw new InvalidOperationException("Refresh token não encontrado.");

            existingToken.IsRevoked = token.IsRevoked;
            existingToken.ReplacedByToken = token.ReplacedByToken;

            await repository.UpdateAsync(existingToken, cancellationToken);
        }
    }
}
