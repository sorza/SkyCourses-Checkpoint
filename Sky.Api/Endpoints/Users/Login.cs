
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Users;

namespace Sky.Api.Endpoints.Users
{
    public class Login : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/login", HandleAsync)
            .WithName("Login")
            .WithSummary("Realiza autenticação do usuário.")
            .WithDescription("Autentica o usuário e retorna um token JWT.")
            .AllowAnonymous();

        private static async Task<IResult> HandleAsync(IUserService service, AuthRequest authDto, CancellationToken cancellationToken = default)
        {
           var result = await service.Login(authDto, cancellationToken);

            return result.Code switch
            {
                200 => TypedResults.Ok(result),
                401 => TypedResults.Unauthorized(),
                423 => TypedResults.Json(result, statusCode: 423),
                _ => TypedResults.BadRequest(result)
            };
        }
    }
}
