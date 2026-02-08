
using Microsoft.AspNetCore.Http.HttpResults;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Courses;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;

namespace Sky.Api.Endpoints.Courses
{
    public class Create : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("CreateCourse")
            .WithSummary("Cria um novo curso.")
            .WithDescription("Cadastra um novo curso no sistema.")
            .RequireAuthorization(policy => policy.RequireRole("Admin","Instructor"));

        public static async Task<Results<
            Created<CreateCourseResponse>,
            BadRequest<Response<CreateCourseResponse>>
            >>
            HandleAsync(
            CourseRequest request,
            ICourseService service,
            CancellationToken cancellationToken = default
            )
        {
            var result = await service.CreateCourseAsync(request, cancellationToken);

            return result.Code switch
            {
                201 => TypedResults.Created($"/courses/{result.Data!.Id}", result.Data),
                _ => TypedResults.BadRequest(result)
            };
        }
    }
}
