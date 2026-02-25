using Sky.Api.Application.Requests.Students;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Students;

namespace Sky.Api.Application.Interfaces
{
    public interface IStudentService
    {
        Task<Response<StudentResponse>?> CreateStudentAsync(CreateStudentRequest request, CancellationToken cancellationToken = default);
        Task<Response<StudentResponse>?> UpdateStudentAsync(int id, CreateStudentRequest request, CancellationToken cancellationToken = default);
        Task<Response<StudentResponse>?> GetStudentByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResponse<IEnumerable<StudentResponse>>?> GetAllStudentsAsync(GetAllStudentsRequest request,CancellationToken cancellationToken = default);
        Task<Response<StudentResponse>?> DeleteStudentAsync(int id, CancellationToken cancellationToken = default);
    }
}
