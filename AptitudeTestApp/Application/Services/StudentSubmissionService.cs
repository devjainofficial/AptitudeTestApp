using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Shared.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public class StudentSubmissionService(IRepository repository, ITestSessionService testSessionService) 
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

    public async Task<int> GetTotalSubmissionCountAsync(Guid creatorId)
    {
        return await Repo.GetQueryable<StudentSubmission>()
                .Where(t => t.TestSession.CreatorId == creatorId)
                .Include(s => s.TestSession)
                .CountAsync();
    }

    public async Task<StudentSubmissionDto?> GetSubmissionDetailsByIdAsync(Guid submissionId)
    {
        var submission = await Repo.GetQueryable<StudentSubmission>()
        .Where(s => s.Id == submissionId)
        .Include(s => s.TestSession).ThenInclude(u => u.University)
        .Include(s => s.AntiCheatLogs)
        .Include(s => s.StudentAnswers).ThenInclude(q => q.Question)
        .Include(s => s.StudentAnswers).ThenInclude(sa => sa.SelectedOption)
        .FirstOrDefaultAsync();

        return submission?.Adapt<StudentSubmissionDto>();
    }

    public async Task<StudentSubmissionDto> StartTestAsync(StudentTestStartDto dto)
    {
        TestSessionDto? testSession = await testSessionService.GetTestSessionByTokenAsync(dto.Token)
            ?? throw new InvalidOperationException("Invalid test session token");

        // Check if student already has a submission
        StudentSubmission? existingSubmission = await Repo.GetQueryable<StudentSubmission>()
            .FirstOrDefaultAsync(s => s.TestSessionId == testSession.Id &&
                                     s.StudentEmail == dto.StudentEmail);

        if (existingSubmission != null)
            return existingSubmission.Adapt<StudentSubmissionDto>();

        var submission = new StudentSubmission
        {
            TestSessionId = testSession.Id,
            StudentName = dto.StudentName,
            StudentEmail = dto.StudentEmail,
            IpAddress = dto.IpAddress,
            BrowserInfo = dto.BrowserInfo,
            StartTime = DateTime.UtcNow,
            Status = TestStatus.InProgress,
        };

        await Repo.AddAsync<StudentSubmission>(submission);

        return submission.Adapt<StudentSubmissionDto>();
    }

    public async Task<StudentSubmissionDto> SubmitTestAsync(Guid submissionId, string reason = "Completed")
    {
        var submission = await Repo.GetQueryable<StudentSubmission>()
            .Include(s => s.StudentAnswers)
                .ThenInclude(sa => sa.Question)
            .Include(s => s.TestSession)
                .ThenInclude(ts => ts.TestSessionQuestions)
                    .ThenInclude(tsq => tsq.Question)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new InvalidOperationException("Submission not found");

        submission.EndTime = DateTime.UtcNow;
        submission.TimeTaken = (int)(submission.EndTime.Value - submission.StartTime).TotalSeconds;

        submission.Status = reason == "Completed"
            ? TestStatus.Completed
            : TestStatus.Disqualified;

        if (reason != TestStatus.Completed.ToString())
        {
            submission.IsDisqualified = true;
            submission.DisqualificationReason = reason;
        }

        // Calculate total score from answers
        submission.TotalScore = submission.StudentAnswers.Sum(sa => sa.PointsEarned);

        // Calculate max possible score from all assigned questions
        submission.MaxPossibleScore = submission.TestSession.TestSessionQuestions
            .Sum(tsq => tsq.Question.Points);

        // Calculate percentage score
        submission.PercentageScore = submission.MaxPossibleScore > 0
            ? (submission.TotalScore / submission.MaxPossibleScore) * 100
            : 0;

        await Repo.SaveChangesAsync();

        return submission.Adapt<StudentSubmissionDto>();
    }
}
