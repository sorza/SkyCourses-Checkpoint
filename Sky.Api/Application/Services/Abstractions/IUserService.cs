using Sky.Api.Application.Dto;
using Sky.Api.Application.Responses;

namespace Sky.Api.Application.Services.Abstractions
{
    public interface IUserService
    {
        Task<Response<ReadUserDto>> Register(CreateUserDto userDto, CancellationToken cancellationToken = default);
    }
}
