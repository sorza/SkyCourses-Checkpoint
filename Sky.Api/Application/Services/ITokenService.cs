namespace Sky.Api.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email, IEnumerable<string> roles);
    }
}
