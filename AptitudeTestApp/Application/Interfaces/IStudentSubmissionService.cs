using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Data.Models;

namespace AptitudeTestApp.Application.Interfaces;

public interface IStudentSubmissionService : IEntityService<StudentSubmissionDto, Guid>
{
    Task<List<StudentSubmissionDto>> GetAllWithTestSessionAsync(Guid creatorId);
    Task<int> GetTotalSubmissionCountAsync(Guid creatorId);
    Task<StudentSubmissionDto?> GetSubmissionDetailsByIdAsync(Guid submissionId);
    Task<StudentSubmissionDto> SubmitTestAsync(Guid submissionId, string reason);
    Task<StudentSubmissionDto> StartTestAsync(StudentTestStartDto dto);

}
