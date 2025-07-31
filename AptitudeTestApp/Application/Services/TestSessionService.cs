using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Infrastructure.Persistence;
using AptitudeTestApp.Shared.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AptitudeTestApp.Application.Services;

public class TestSessionService(IRepository Repo) 
    : EntityService<TestSessionDto, TestSession>(Repo), 
      ITestSessionService
{
    public async Task<TestSessionDto> CreateTestSessionAsync(CreateTestSessionDto dto, Guid creatorId)
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
            CreatorId = creatorId
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

        return testSession.Adapt<TestSessionDto>();
    }

    public async Task<TestSessionDto?> GetTestSessionByTokenAsync(string token)
    {
        TestSession? testSession = await Repo.GetQueryable<TestSession>()
            .Include(ts => ts.University)
            .FirstOrDefaultAsync(ts => ts.Token == token && ts.IsActive);

        return testSession?.Adapt<TestSessionDto>();
    }

    public async Task<List<TestSessionDto>> GetTestSessionsByUniversityAsync(Guid universityId)
    {
        List<TestSession> testSession = await Repo.GetQueryable<TestSession>()
            .Include(ts => ts.University)
            .Where(ts => ts.UniversityId == universityId)
            .OrderByDescending(ts => ts.CreatedAt)
            .ToListAsync();

        return testSession.Adapt<List<TestSessionDto>>();
    }

    public async Task<bool> IsTestSessionActiveAsync(string token)
    {
        TestSessionDto? testSession = await GetTestSessionByTokenAsync(token);
        if (testSession == null) return false;

        DateTime now = DateTime.UtcNow;
        return testSession.IsActive &&
               now >= testSession.StartDate &&
               now <= testSession.EndDate;
    }

    public async Task<List<QuestionDto>> GetRandomizedQuestionsAsync(Guid submissionId)
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

        return questions;
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

    private static string GenerateUniqueToken()
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

    public async Task<int> GetActiveTestCountAsync(Guid creatorId)
    {
        return await Repo.GetQueryable<TestSession>()
            .CountAsync(ts => ts.IsActive &&
                        ts.CreatorId == creatorId && 
                        ts.StartDate <= DateTime.UtcNow && 
                        ts.EndDate >= DateTime.UtcNow
            );
    }

    public async Task<List<TestSession>> GetRecentTestSessionsAsync(int days, Guid creatorId)
    {
        DateTime thresholdDate = DateTime.UtcNow.AddDays(-days);

        return await Repo.GetQueryable<TestSession>()
            .Include(ts => ts.University)
            .Include(s => s.StudentSubmissions)
            .Where(ts => ts.CreatedAt >= thresholdDate && ts.CreatorId == creatorId)
            .OrderByDescending(ts => ts.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<TestSessionDto>> GetAllTestSessionWithUniversityAsync(Guid creatorId)
    {
       List<TestSession> testSessions = await Repo.GetQueryable<TestSession>()
            .Where(t => t.CreatorId == creatorId)
            .Include(ts => ts.University)
            .ToListAsync();

        return testSessions.Adapt<List<TestSessionDto>>();
    }

    public async Task ToggleActiveTestSessionAsync(Guid id)
    {
        var session = await Repo.GetByIdAsync<TestSession>(id);
        if (session != null)
        {
            session.IsActive = !session.IsActive;
            await Repo.SaveChangesAsync();
        }
    }
}
