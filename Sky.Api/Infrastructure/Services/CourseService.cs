using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Courses;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;
using Sky.Api.Domain.Entities;

namespace Sky.Api.Infrastructure.Services
{
    public class CourseService(IRepository<Course> repository) : ICourseService
    {
        public async Task<Response<CreateCourseResponse>> CreateCourseAsync(CourseRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
                return new Response<CreateCourseResponse>(null, 400, "Preencha as informações do curso.");

            Course course;

            try
            {
                course = Course.Create(request.Title, request.Description, request.Category, request.Workload);               
            }
            catch(ArgumentException ex)
            {
                return new Response<CreateCourseResponse>(null, 400, ex.Message);
            }           

            await repository.CreateAsync(course, cancellationToken);    

            var response = new CreateCourseResponse
            (
                course.Id,
                course.Title,
                course.Description,
                course.Category,
                course.Workload,
                course.CreatedAt
            );

            return new Response<CreateCourseResponse>(response,201,"Curso cadastrado com sucesso!");

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

        public Task<Response<CourseResponse>> UpdateCourseAsync(int id, CourseRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
