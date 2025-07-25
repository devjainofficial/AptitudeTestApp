using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Infrastructure.Persistence;
using AptitudeTestApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AptitudeTestApp.Application.Services;

public class TestSessionService(IRepository Repo) : ITestSessionService
{
    public async Task<TestSession> CreateTestSessionAsync(CreateTestSessionDto dto, string createdBy)
    {
        TestSession testSession = new ()
        {
            UniversityId = dto.UniversityId,
            TestName = dto.TestName,
            Description = dto.Description,
            TimeLimit = dto.TimeLimit,
            TotalQuestions = dto.TotalQuestions,
            PassingScore = dto.PassingScore,
            MaxTabSwitches = dto.MaxTabSwitches,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Token = GenerateUniqueToken(),
            CreatedBy = createdBy
        };

        await Repo.AddAsync<TestSession>(testSession);

        // Add selected questions to test session with randomized order
        List<TestSessionQuestion> randomizedQuestions = [.. dto.SelectedQuestionIds
            .OrderBy(x => Guid.NewGuid())
            .Take(dto.TotalQuestions)
            .Select((questionId, index) => new TestSessionQuestion
            {
                TestSessionId = testSession.Id,
                QuestionId = questionId,
                DisplayOrder = index + 1
            })
        ];

        await Repo.AddRangeAsync<TestSessionQuestion>(randomizedQuestions);

        return testSession;
    }

    public async Task<TestSession?> GetTestSessionByTokenAsync(string token)
    {
        return await Repo.GetQueryable<TestSession>()
            .Include(ts => ts.University)
            .FirstOrDefaultAsync(ts => ts.Token == token && ts.IsActive);
    }

    public async Task<List<TestSession>> GetTestSessionsByUniversityAsync(Guid universityId)
    {
        return await Repo.GetQueryable<TestSession>()
            .Include(ts => ts.University)
            .Where(ts => ts.UniversityId == universityId)
            .OrderByDescending(ts => ts.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IsTestSessionActiveAsync(string token)
    {
        TestSession? testSession = await GetTestSessionByTokenAsync(token);
        if (testSession == null) return false;

        DateTime now = DateTime.UtcNow;
        return testSession.IsActive &&
               now >= testSession.StartDate &&
               now <= testSession.EndDate;
    }

    public async Task<StudentSubmission> StartTestAsync(StudentTestStartDto dto)
    {
        TestSession? testSession = await GetTestSessionByTokenAsync(dto.Token)
            ?? throw new InvalidOperationException("Invalid test session token");

        // Check if student already has a submission
        StudentSubmission? existingSubmission = await Repo.GetQueryable<StudentSubmission>()
            .FirstOrDefaultAsync(s => s.TestSessionId == testSession.Id &&
                                     s.StudentEmail == dto.StudentEmail);

        if (existingSubmission != null)
            return existingSubmission;

        var submission = new StudentSubmission
        {
            TestSessionId = testSession.Id,
            StudentName = dto.StudentName,
            StudentEmail = dto.StudentEmail,
            StartTime = DateTime.UtcNow,
            Status = TestStatus.InProgress,
        };

        await Repo.AddAsync<StudentSubmission>(submission);

        return submission;
    }

    public async Task<(List<QuestionDto>, StudentSubmission)> GetRandomizedQuestionsWithSubmissionAsync(Guid submissionId)
    {
        StudentSubmission? submission = await Repo.GetQueryable<StudentSubmission>()
            .Include(s => s.TestSession)
            .ThenInclude(ts => ts.TestSessionQuestions)
            .ThenInclude(tsq => tsq.Question)
            .ThenInclude(q => q.Options)
            .Include(s => s.TestSession)
            .ThenInclude(ts => ts.TestSessionQuestions)
            .ThenInclude(tsq => tsq.Question)
            .ThenInclude(q => q.Category)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new InvalidOperationException("Submission not found");

        List<QuestionDto> questions = [.. submission.TestSession.TestSessionQuestions
            .OrderBy(tsq => tsq.DisplayOrder)
            .Select(tsq => new QuestionDto
            {
                Id = tsq.Question.Id,
                QuestionText = tsq.Question.QuestionText,
                Points = tsq.Question.Points,
                CategoryName = tsq.Question.Category.Name,
                Options = [.. tsq.Question.Options
                    .OrderBy(o => Guid.NewGuid()) // Randomize options
                    .Select(o => new QuestionOptionDto
                    {
                        Id = o.Id,
                        OptionText = o.OptionText
                    })]
            })];

        return (questions, submission);
    }

    public async Task SaveAnswerAsync(Guid submissionId, Guid questionId, Guid? selectedOptionId)
    {
        var existingAnswer = await Repo.GetQueryable<StudentAnswer>()
            .FirstOrDefaultAsync(sa => sa.SubmissionId == submissionId &&
                                       sa.QuestionId == questionId);

        if (existingAnswer != null)
        {
            // Update existing answer
            existingAnswer.SelectedOptionId = selectedOptionId;
            existingAnswer.AnsweredAt = DateTime.UtcNow;
        }
        else
        {
            // Create new answer
            existingAnswer = new StudentAnswer
            {
                SubmissionId = submissionId,
                QuestionId = questionId,
                SelectedOptionId = selectedOptionId,
                AnsweredAt = DateTime.UtcNow
            };

            await Repo.AddAsync(existingAnswer, false);
        }

        // Calculate correctness and points
        if (selectedOptionId.HasValue)
        {
            var selectedOption = await Repo.GetQueryable<QuestionOption>()
                .Include(qo => qo.Question)
                .FirstOrDefaultAsync(qo => qo.Id == selectedOptionId.Value);

            if (selectedOption != null)
            {
                existingAnswer.IsCorrect = selectedOption.IsCorrect;
                existingAnswer.PointsEarned = selectedOption.IsCorrect ? selectedOption.Question.Points : 0;
            }
        }

        await Repo.SaveChangesAsync();
    }


    public async Task<StudentSubmission> SubmitTestAsync(Guid submissionId, string reason = "Completed")
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
        return submission;
    }



    public async Task<StudentSubmission?> GetSubmissionAsync(Guid submissionId)
    {
        return await Repo.GetQueryable<StudentSubmission>()
            .Include(s => s.TestSession)
                .ThenInclude(ts => ts.University)
            .Include(s => s.StudentAnswers)
            .FirstOrDefaultAsync(s => s.Id == submissionId);
    }


    public string GenerateUniqueToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes)
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "")
            .Substring(0, 12);
    }

    public async Task<int> GetActiveTestCountAsync()
    {
        return await Repo.GetQueryable<TestSession>()
            .CountAsync(ts => ts.IsActive && ts.StartDate <= DateTime.UtcNow && ts.EndDate >= DateTime.UtcNow);
    }

    public Task<int> GetTotalSubmissionCountAsync()
    {
        return Repo.GetQueryable<StudentSubmission>()
            .CountAsync(s => s.Status == TestStatus.Completed || s.Status == TestStatus.Disqualified);
    }

    public Task<List<TestSession>> GetRecentTestSessionsAsync(int days)
    {
        DateTime thresholdDate = DateTime.UtcNow.AddDays(-days);

        return Repo.GetQueryable<TestSession>()
            .Include(ts => ts.University)
            .Include(s => s.StudentSubmissions)
            .Where(ts => ts.CreatedAt >= thresholdDate)
            .OrderByDescending(ts => ts.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TestSession>> GetAllActiveTestSessionAsync()
    {
        return await Repo.GetQueryable<TestSession>()
            .Include(ts => ts.University)
            .Where(t => t.IsActive)
            .ToListAsync();
    }

    public async Task DeactivateTestSessionAsync(Guid id)
    {
        var session = await Repo.GetByIdAsync<TestSession>(id);
        if (session != null)
        {
            session.IsActive = false;
            await Repo.SaveChangesAsync();
        }
    }

    public async Task DeleteTestSessionAsync(Guid id)
    {
        await Repo.DeleteAsync<TestSession>(id);
        await Repo.SaveChangesAsync();
    }

}
