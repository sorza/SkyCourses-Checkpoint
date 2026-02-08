namespace Sky.Api.Application.Requests.Courses
{
    public sealed record CourseRequest(string Title, string Description, string Category, int Workload);    
}