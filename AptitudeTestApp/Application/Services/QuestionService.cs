using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;

namespace AptitudeTestApp.Application.Services;

public class QuestionService(IRepository Repo) : IQuestionService
{
    public Task AddQuestionAsync(Question question)
    {
        if (question == null) throw new ArgumentNullException(nameof(question));

        return Repo.AddAsync<Question>(question);
    }

    public Task DeleteQuestionAsync(Guid questionId)
    {
        if (questionId == Guid.Empty) throw new ArgumentException("Invalid question ID", nameof(questionId));

        return Repo.DeleteAsync<Question>(questionId);
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        var questions = await Repo.GetAllAsync<Question>();
        
        return [.. questions];
    }

    public Task<int> GetTotalCountAsync()
    {
        return Repo.GetAllAsync<Question>().ContinueWith(t => t.Result.Count());
    }

    public Task UpdateQuestionAsync(Question question)
    {
        if (question == null) throw new ArgumentNullException(nameof(question));

        if (question.Id == Guid.Empty) throw new ArgumentException("Invalid question ID", nameof(question.Id));
        
        return Repo.UpdateAsync<Question>(question);
    }
}
