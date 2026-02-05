using Microsoft.AspNetCore.Identity;
using Sky.Api.Application.Dto;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Services.Abstractions;
using Sky.Api.Domain.ValueObjects;

namespace Sky.Api.Application.Services.Implementation
{
    public class UserService(UserManager<IdentityUser> userManager) : IUserService
    {
        public async Task<Response<ReadUserDto>> Register(CreateUserDto userDto, CancellationToken cancellationToken = default)
        {
            if (userDto is null)
                return new Response<ReadUserDto>(null, 400, "Os dados do usuário são obrigatórios.");           

            try
            {
                var email = Email.Create(userDto.Email);
            }
            catch(ArgumentException ex)
            {
                return new Response<ReadUserDto>(null, 400, ex.Message);
            }

            var user = new IdentityUser
            {
                Email = userDto.Email,
                UserName = userDto.Email,
            };

            var result = await userManager.CreateAsync(user, userDto.Password);

            if(!result.Succeeded)
                return new Response<ReadUserDto>(null, 400, string.Join("; ", result.Errors.Select(e => e.Description)));

            return new Response<ReadUserDto>(
                new ReadUserDto(user.Id, user.Email,"Student"));
        }
    }
}
