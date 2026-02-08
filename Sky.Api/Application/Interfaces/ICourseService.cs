using Sky.Api.Application.Requests.Courses;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;

namespace Sky.Api.Application.Interfaces
{
    public interface ICourseService
    {
        Task<Response<CreateCourseResponse>> CreateCourseAsync(CourseRequest request, CancellationToken cancellationToken = default);
        Task<Response<CourseResponse>> GetCourseByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponse<IEnumerable<CourseResponse>>> GetAllCoursesAsync(CancellationToken cancellationToken = default);
        Task<Response<CourseResponse>> UpdateCourseAsync(int id, CourseRequest request, CancellationToken cancellationToken = default);
        Task<Response<bool>> DeleteCourseAsync(int id, CancellationToken cancellationToken = default);
    }
}
