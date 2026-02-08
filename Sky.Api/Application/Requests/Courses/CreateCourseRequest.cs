namespace Sky.Api.Application.Requests.Courses
{
    public sealed record CreateCourseRequest(string Title, string Description, string Category, int Workload);    
}