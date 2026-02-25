namespace Sky.Api.Application.Responses.Students
{
    public sealed record GetStudentResponse(string UserId, string Name, string Email, DateTime RegistratedAt);
}
