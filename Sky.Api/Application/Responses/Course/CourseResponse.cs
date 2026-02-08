using Sky.Api.Domain.Entities;

namespace Sky.Api.Application.Responses.Course
{
    public sealed record CourseResponse(int Id, string Title, string Description, string Category, int Workload, DateTime CreatedAt, ICollection<Enrollment> Enrollments);    
}
