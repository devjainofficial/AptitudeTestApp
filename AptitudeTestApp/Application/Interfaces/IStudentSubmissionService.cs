using AptitudeTestApp.Application.DTOs;

namespace AptitudeTestApp.Application.Interfaces;

public interface IStudentSubmissionService : IEntityService<StudentSubmissionDto, Guid>
{
    Task<List<StudentSubmissionDto>> GetAllWithTestSessionAsync(Guid CreatorId);
}
