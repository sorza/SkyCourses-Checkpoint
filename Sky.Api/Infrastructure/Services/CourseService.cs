using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Courses;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;
using Sky.Api.Domain.Entities;

namespace Sky.Api.Infrastructure.Services
{
    public class CourseService(IRepository<Course> repository) : ICourseService
    {
        public async Task<Response<CourseResponse>> CreateCourseAsync(CreateCourseRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
                return new Response<CourseResponse>(null, 400, "Preencha as informações do curso.");

            Course course;

            try
            {
                course = Course.Create(request.Title, request.Description, request.Category, request.Workload);               
            }
            catch(ArgumentException ex)
            {
                return new Response<CourseResponse>(null, 400, ex.Message);
            }           

            await repository.CreateAsync(course, cancellationToken);    

            var response = new CourseResponse
            (
                course.Id,
                course.Title,
                course.Description,
                course.Category,
                course.Workload,
                course.CreatedAt
            );

            return new Response<CourseResponse>(response,201,"Curso cadastrado com sucesso!");

        }

        public Task<Response<bool>> DeleteCourseAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<IEnumerable<CourseResponse>>> GetAllCoursesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CourseResponse>> GetCourseByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Response<CourseResponse>> UpdateCourseAsync(int id, CreateCourseRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
