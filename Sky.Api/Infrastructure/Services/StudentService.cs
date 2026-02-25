using Sky.Api.Application.Interfaces;
using Sky.Api.Application.Requests.Students;
using Sky.Api.Application.Responses;
using Sky.Api.Application.Responses.Students;

namespace Sky.Api.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        public async Task<Response<StudentResponse>?> CreateStudentAsync(CreateStudentRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<StudentResponse>?> DeleteStudentAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<IEnumerable<StudentResponse>>?> GetAllStudentsAsync(GetAllStudentsRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<StudentResponse>?> GetStudentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<StudentResponse>?> UpdateStudentAsync(int id, CreateStudentRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
