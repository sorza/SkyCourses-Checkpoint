using Sky.Api.Application.Requests.Users;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Users;

namespace Sky.Api.Application.Services
{
    public interface IUserService
    {
        Task<Response<UserResponse>> Register(UserRequest request, CancellationToken cancellationToken = default);
        Task<Response<AuthResponse>> Login(AuthRequest request, CancellationToken cancellationToken = default);
    }
}
