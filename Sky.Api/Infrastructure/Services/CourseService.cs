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

        public async Task<PagedResponse<IEnumerable<CourseResponse>>> GetAllCoursesAsync(GetCoursesRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
                return new PagedResponse<IEnumerable<CourseResponse>>(null, 400, "Parâmetros de pesquisa inválidos");
           
            if (request.PageNumber < 1)
                return new PagedResponse<IEnumerable<CourseResponse>>(null, 400, "O número da página deve ser maior que zero.");

            if (request.PageSize < 1 || request.PageSize > 100)
                return new PagedResponse<IEnumerable<CourseResponse>>(null, 400, "O tamanho da página deve estar entre 1 e 100.");

            IEnumerable<Course>? courses;

            try
            {               
                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    courses = await repository.GetAllAsync(
                        c => c.Category.ToLower() == request.Category.ToLower(),
                        cancellationToken
                    );
                }
                else
                {                  
                    courses = await repository.GetAllAsync(cancellationToken);
                }

                if (courses is null || !courses.Any())
                    return new PagedResponse<IEnumerable<CourseResponse>>(
                        new List<CourseResponse>(),
                        200,
                        "Nenhum curso encontrado."
                    );
              
                var totalRecords = courses.Count();
              
                var pagedCourses = courses
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new CourseResponse(
                        c.Id,
                        c.Title,
                        c.Description,
                        c.Category,
                        c.Workload,
                        c.CreatedAt
                    ))
                    .ToList();

                return new PagedResponse<IEnumerable<CourseResponse>>(
                    pagedCourses,
                    totalRecords,
                    request.PageNumber,
                    request.PageSize
                );
            }
            catch (Exception ex)
            {
                return new PagedResponse<IEnumerable<CourseResponse>>(
                    null,
                    500,
                    $"Erro ao buscar cursos: {ex.Message}"
                );
            }
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
