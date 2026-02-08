
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Users;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Users;
using Sky.Api.Infrastructure.Repositories;

namespace Sky.Api.Endpoints.Users
{
    public class RefreshToken : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/refresh-token", HandleAsync)
            .WithName("RefreshToken")
            .WithSummary("Atualiza o access token usando refresh token.")
            .Produces<Ok<AuthResponse>>(200)
            .Produces<UnauthorizedHttpResult>(401)
            .AllowAnonymous();

        private static async Task<IResult> HandleAsync(
            RefreshTokenRequest request,
            ITokenService service, 
            UserManager<IdentityUser> userManager,
            CancellationToken cancellationToken = default)
        {
            var userId = await service.ValidateRefreshToken(request.RefreshToken);

            if (userId is null)
                return TypedResults.Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return TypedResults.Unauthorized();

            var roles = await userManager.GetRolesAsync(user);

            var newAccessToken = service.GenerateToken(user.Id, user.Email!, roles);
            var newRefreshToken = service.GenerateRefreshToken();

            var accessTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var oldToken = await service.GetRefreshToken(request.RefreshToken, cancellationToken);

            if (oldToken is not null)
            {
                oldToken.IsRevoked = true;
                oldToken.ReplacedByToken = newRefreshToken;
                await service.UpdateRefreshToken(oldToken, cancellationToken);
            }            

            var newRefreshTokenEntity = new Domain.Entities.RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = refreshTokenExpiry,
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            var response = new AuthResponse(
                user.Id,
                user.Email!,
                newAccessToken,
                newRefreshToken,
                accessTokenExpiry,
                refreshTokenExpiry,
                roles
            );

            await service.SaveRefreshToken(newRefreshTokenEntity, cancellationToken);

            return TypedResults.Ok(new Response<AuthResponse>(response, 200, "Token atualizado com sucesso."));

        }
    }
}
