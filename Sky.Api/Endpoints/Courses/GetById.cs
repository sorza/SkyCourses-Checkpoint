using Microsoft.AspNetCore.Http.HttpResults;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Courses;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;

namespace Sky.Api.Endpoints.Courses
{
    public class GetById : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id:int}", HandleAsync)
            .WithName("GetCourseById")
            .WithSummary("Obtém um curso por ID.")
            .WithDescription("Recupera os detalhes de um curso específico usando seu ID.")
            .AllowAnonymous();

        public static async Task<Results<
            Ok<CourseResponse>,
            NotFound<Response<CourseResponse>>
            >>
            HandleAsync(
            int id,
            ICourseService service,
            CancellationToken cancellationToken = default
            )
        {
            var result = await service.GetCourseByIdAsync(id, cancellationToken);

            return result.IsSuccess
                 ? TypedResults.Ok(result.Data)
                 : TypedResults.NotFound(result);
        }

    }
}
