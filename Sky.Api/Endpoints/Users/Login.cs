
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Users;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Users;

namespace Sky.Api.Endpoints.Users
{
    public class Login : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/login", HandleAsync)
            .WithName("Login")
            .WithSummary("Realiza autenticação do usuário.")
            .WithDescription("Autentica o usuário e retorna um token JWT.")
            .Produces<Ok<AuthResponse>>(200)
            .Produces<BadRequest<Response<AuthResponse>>>(400)
            .Produces<UnauthorizedHttpResult>(401)
            .Produces<ForbidHttpResult>(403)
            .AllowAnonymous();

        private static async Task<IResult> HandleAsync(IUserService service, AuthRequest authDto, CancellationToken cancellationToken = default)
        {
           var result = await service.Login(authDto, cancellationToken);

            return result.Code switch
            {
                200 => TypedResults.Ok(result),
                401 => TypedResults.Unauthorized(),
                403 => TypedResults.Json(result, statusCode: 403),
                _ => TypedResults.BadRequest(result)
            };
        }
    }
}
