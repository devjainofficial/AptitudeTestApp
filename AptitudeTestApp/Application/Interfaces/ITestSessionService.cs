using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Shared.Enums;

namespace AptitudeTestApp.Application.Interfaces;

public interface ITestSessionService : IEntityService<TestSessionDto, Guid>
{
    Task<TestSessionDto> CreateTestSessionAsync(CreateTestSessionDto dto, Guid creatorId);
    Task<TestSessionDto?> GetTestSessionByTokenAsync(string token);
    Task<List<TestSessionDto>> GetTestSessionsByUniversityAsync(Guid universityId);
    Task<List<TestSessionDto>> GetAllTestSessionWithUniversityAsync(Guid creatorId);
    Task<List<TestSession>> GetRecentTestSessionsAsync(int days, Guid creatorId);
    Task<List<QuestionDto>> GetRandomizedQuestionsAsync(Guid submissionId);
    Task<int> GetActiveTestCountAsync(Guid creatorId);
    Task<bool> IsTestSessionActiveAsync(string token);
    Task SaveAnswerAsync(Guid submissionId, Guid questionId, Guid? selectedOptionId);
    Task ToggleActiveTestSessionAsync(Guid id);
    Task<List<TestSessionDto>?> GetTestsByQuestionIdAsync(Guid questionId);
}