using Sky.Api.Domain.Entities;

namespace Sky.Api.Application.Responses.Course
{
    public sealed record CreateCourseResponse(int Id, string Title, string Description, string Category, int Workload, DateTime CreatedAt);    
}
