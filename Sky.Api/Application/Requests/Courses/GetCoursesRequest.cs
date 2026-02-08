namespace Sky.Api.Application.Requests.Courses
{
    public sealed record GetCoursesRequest(string? Category = null, int PageNumber = 1, int PageSize = 10);
    
}
