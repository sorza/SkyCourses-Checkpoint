using Microsoft.AspNetCore.Identity;
using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Users;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Users;
using Sky.Api.Domain.ValueObjects;

namespace Sky.Api.Infrastructure.Implementation
{
    public class UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService) : IUserService
    {
        public async Task<Response<AuthResponse>> Login(AuthRequest request, CancellationToken cancellationToken = default)
        {
            if(request is null)
                return new Response<AuthResponse>(null, 400, "Os dados de autenticação são obrigatórios.");

            try
            {
                var email = Email.Create(request.Email);
            }
            catch (ArgumentException ex)
            {
                return new Response<AuthResponse>(null, 400, ex.Message);
            }

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return new Response<AuthResponse>(null, 404, "Usuário ou senha inválidos.");

            var result = await signInManager.PasswordSignInAsync(
                user,
                request.Password,
                isPersistent: false,
                lockoutOnFailure: true
            );

            if(result.IsLockedOut)
                return new Response<AuthResponse>(null, 403, "Usuário temporariamente bloqueado. Tente novamente mais tarde.");

            if (!result.Succeeded)
                return new Response<AuthResponse>(null, 404, "Usuário ou senha inválidos.");

            var roles = await userManager.GetRolesAsync(user);

            var token = tokenService.GenerateToken(user.Id, user.Email!, roles);           

            var response = new AuthResponse
            (
                user.Id,
                user.Email!,
                token,                
                roles
            );

            return new Response<AuthResponse>(response, 200, "Login realizado com sucesso.");

        }

        public async Task<Response<UserResponse>> Register(UserRequest userDto, CancellationToken cancellationToken = default)
        {
            if (userDto is null)
                return new Response<UserResponse>(null, 400, "Os dados do usuário são obrigatórios.");           

            try
            {
                var email = Email.Create(userDto.Email);
            }
            catch(ArgumentException ex)
            {
                return new Response<UserResponse>(null, 400, ex.Message);
            }

            var user = new IdentityUser
            {
                Email = userDto.Email,
                UserName = userDto.Email,
            };

            var result = await userManager.CreateAsync(user, userDto.Password);

            if(!result.Succeeded)
                return new Response<UserResponse>(null, 400, string.Join("; ", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(user, "Student");

            return new Response<UserResponse>(
                new UserResponse(user.Id, user.Email,"Student"));
        }
    }
}
