namespace Sky.Api.Application.Responses.Users
{
    public record AuthResponse(
        string UserId,
        string Email,
        string Token,
        string RefreshToken, 
        DateTime TokenExpiresAt,
        DateTime RefreshTokenExpiresAt,
        IEnumerable<string> Roles
    );
}
