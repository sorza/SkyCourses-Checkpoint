namespace Sky.Api.Application.Responses.Students
{
    public sealed record StudentResponse(string UserId, string Name, string Email, DateTime RegistratedAt);
}
