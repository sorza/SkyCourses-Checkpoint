namespace Sky.Api.Application.Requests.Students
{
    public sealed record CreateStudentRequest(string UserId, string Name, string Email);
    
}
