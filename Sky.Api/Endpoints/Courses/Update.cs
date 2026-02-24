using Microsoft.AspNetCore.Http.HttpResults;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Courses;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Course;

namespace Sky.Api.Endpoints.Courses
{
    public class Update : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:int}", HandleAsync)
            .WithName("UpdateCourse")
            .WithSummary("Atualiza um curso existente.")
            .WithDescription("Atualiza os detalhes de um curso específico usando seu ID.")
            .RequireAuthorization(policy => policy.RequireRole("Admin","Instructor"));
    

        public static async Task<Results<
            Ok<CourseResponse>,
            BadRequest<Response<CourseResponse>>,
            NotFound<Response<CourseResponse>>
            >>
            HandleAsync(
            int id,
            CreateCourseRequest request,
            ICourseService service,
            CancellationToken cancellationToken = default
            )
        {
            var result = await service.UpdateCourseAsync(id, request, cancellationToken);
            return result.Code switch
            {
                200 => TypedResults.Ok(result.Data),               
                404 => TypedResults.NotFound(result),
                _ => TypedResults.BadRequest(result)
            };
        }

    }
}
