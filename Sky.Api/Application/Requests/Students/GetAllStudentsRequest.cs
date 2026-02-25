namespace Sky.Api.Application.Requests.Students
{
    public sealed record GetAllStudentsRequest(int PageNumber = 1, int PageSize = 10);  
    
}
