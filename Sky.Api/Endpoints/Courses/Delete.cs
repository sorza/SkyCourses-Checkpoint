using Microsoft.AspNetCore.Http.HttpResults;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;

namespace Sky.Api.Endpoints.Courses
{
    public class Delete : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id:int}", HandleAsync)
            .WithName("DeleteCourse")
            .WithSummary("Exclui um curso existente.")
            .WithDescription("Remove um curso específico usando seu ID.")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        public static async Task<Results<
            NoContent,
            NotFound<Response<CourseResponse>>
            >>
            HandleAsync(
            int id,
            ICourseService service,
            CancellationToken cancellationToken = default
            )
        {
            var result = await service.DeleteCourseAsync(id, cancellationToken);

            return result.IsSuccess
                 ? TypedResults.NoContent()
                 : TypedResults.NotFound(result);
        }
    }
}
