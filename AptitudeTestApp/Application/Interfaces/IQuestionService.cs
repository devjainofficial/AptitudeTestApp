using AptitudeTestApp.Data.Models;

namespace AptitudeTestApp.Application.Interfaces;

public interface IQuestionService
{
    Task<List<Question>> GetAllQuestionsAsync();
    Task AddQuestionAsync(Question question);
    Task UpdateQuestionAsync(Question question);
    Task DeleteQuestionAsync(Guid questionId);
    Task<int> GetTotalCountAsync();
}
