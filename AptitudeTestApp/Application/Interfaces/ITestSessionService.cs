using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Shared.Enums;

namespace AptitudeTestApp.Application.Interfaces;

public interface ITestSessionService
{
    Task<TestSession> CreateTestSessionAsync(CreateTestSessionDto dto, Guid creatorId);
    Task<TestSession?> GetTestSessionByTokenAsync(string token);
    Task<List<TestSession>> GetTestSessionsByUniversityAsync(Guid universityId);
    Task<bool> IsTestSessionActiveAsync(string token);
    Task<StudentSubmission> StartTestAsync(StudentTestStartDto dto);
    Task<(List<QuestionDto>, StudentSubmission)> GetRandomizedQuestionsWithSubmissionAsync(Guid submissionId);
    Task SaveAnswerAsync(Guid submissionId, Guid questionId, Guid? selectedOptionId);
    Task<StudentSubmission> SubmitTestAsync(Guid submissionId, string reason);
    Task<StudentSubmission?> GetSubmissionAsync(Guid submissionId);
    Task<int> GetActiveTestCountAsync();
    Task<int> GetTotalSubmissionCountAsync();
    Task<List<TestSession>> GetAllTestSessionAsync();
    Task ToggleActiveTestSessionAsync(Guid id);
    Task DeleteTestSessionAsync(Guid id);
    Task<List<TestSession>> GetRecentTestSessionsAsync(int days);
    string GenerateUniqueToken();
}