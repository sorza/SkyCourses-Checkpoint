using Microsoft.AspNetCore.Identity;
using Sky.Api.Application.Requests.Users;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Users;
using Sky.Api.Application.Services;
using Sky.Api.Domain.ValueObjects;

namespace Sky.Api.Infrastructure.Services
{
    public class UserService(UserManager<IdentityUser> userManager) : IUserService
    {
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

            return new Response<UserResponse>(
                new UserResponse(user.Id, user.Email,"Student"));
        }
    }
}
