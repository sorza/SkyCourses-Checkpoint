using Sky.Api.Domain.Entities;

namespace Sky.Api.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email, IEnumerable<string> roles);
        string GenerateRefreshToken();
        Task<string?> ValidateRefreshToken(string refreshToken);
        Task SaveRefreshToken(RefreshToken token, CancellationToken cancellationToken = default);
        Task UpdateRefreshToken(RefreshToken token, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken = default);
    }
}
