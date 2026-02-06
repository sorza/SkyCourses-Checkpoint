using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Users;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Users;

namespace Sky.Api.Endpoints.Users
{
    public class Register : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Registro")
            .WithSummary("Registra um novo usuário.")
            .WithDescription("Registra um novo usuário na aplicação.");

        private static async Task<IResult> HandleAsync(IUserService service, UserRequest userDto, CancellationToken cancellationToken = default)
        {
           var result = await service.Register(userDto, cancellationToken);

           return result.IsSuccess
                ? Results.Created($"/users/{result.Data!.Id}", result.Data)
                : Results.BadRequest(result);
        }       
    }
}
