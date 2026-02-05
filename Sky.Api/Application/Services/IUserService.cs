using Sky.Api.Application.Requests.Users;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Users;

namespace Sky.Api.Application.Services
{
    public interface IUserService
    {
        Task<Response<UserResponse>> Register(UserRequest userDto, CancellationToken cancellationToken = default);       
    }
}
