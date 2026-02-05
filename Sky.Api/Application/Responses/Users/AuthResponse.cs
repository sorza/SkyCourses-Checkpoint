namespace Sky.Api.Application.Responses.Users
{
    public record AuthResponse(
        string UserId,
        string Email,
        string Token,
        IEnumerable<string> Roles
    );
}
