using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public class StudentSubmissionService(IRepository repository) 
    : EntityService<StudentSubmissionDto, StudentSubmission>(repository), 
      IStudentSubmissionService
{
    public async Task<List<StudentSubmissionDto>> GetAllWithTestSessionAsync(Guid creatorId)
    {
        var query = Repo.GetQueryable<StudentSubmission>()
            .Where(s => s.TestSession.CreatorId == creatorId)
            .Include(s => s.TestSession);

        List<StudentSubmission>? submissions = await query.ToListAsync();
        
        return submissions.Adapt<List<StudentSubmissionDto>>();
    }
}
