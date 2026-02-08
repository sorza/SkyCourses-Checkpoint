
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Courses;

namespace Sky.Api.Endpoints.Courses
{
    public class Create : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/courses", HandleAsync)
            .WithName("CreateCourse")
            .WithSummary("Cria um novo curso.")
            .WithDescription("Cadastra um novo curso no sistema.");

        public async static Task<IResult> HandleAsync(
            CourseRequest request,
            ICourseService service,
            CancellationToken cancellationToken = default
            )
        {
            var result = await service.CreateCourseAsync(request, cancellationToken);

            return result.Code switch
            {
                201 => TypedResults.Created($"/courses/{result.Data!.Id}", result.Data),
                500 => TypedResults.Problem(),
                _ => TypedResults.BadRequest(result)
            };
        }
    }
}
