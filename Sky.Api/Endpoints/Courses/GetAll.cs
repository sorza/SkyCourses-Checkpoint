using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Courses;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;

namespace Sky.Api.Endpoints.Courses
{
    public class GetAll : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("GetAllCourses")
            .WithSummary("Lista todos os cursos.")
            .WithDescription("Retorna uma lista paginada de cursos. Permite filtrar por categoria.")
            .AllowAnonymous(); 

        private static async Task<Results<Ok<PagedResponse<IEnumerable<CourseResponse>>>, BadRequest<PagedResponse<IEnumerable<CourseResponse>>>>> HandleAsync(
            ICourseService service,
            [FromQuery] string? category,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,            
            CancellationToken cancellationToken = default)
        {
            var request = new GetCoursesRequest(category, pageNumber, pageSize);

            var result = await service.GetAllCoursesAsync(request, cancellationToken);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
