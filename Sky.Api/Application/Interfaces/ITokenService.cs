namespace Sky.Api.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email, IEnumerable<string> roles);
    }
}
