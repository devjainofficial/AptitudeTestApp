using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Shared.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public class QuestionService(IRepository repo)
    : EntityService<QuestionDto, Question>(repo), IQuestionService
{
    public async Task<List<QuestionCategoryDto>> GetAllActiveCategoriesAsync()
    {
        List<QuestionCategory>? categories = await Repo.GetQueryable<QuestionCategory>()
            .Where(c => c.IsActive)
            .ToListAsync();

        return categories.Adapt<List<QuestionCategoryDto>>();
    }

    public async Task<(List<QuestionDto> questionsList, int totalQuestions)> GetQuestionsByFiltersAsync(
        Guid creatorId, 
        int skip, 
        int take, 
        bool? IsActive, 
        QuestionDifficulty? questionDifficulty
    )
    {
        var query = Repo.GetQueryable<Question>()
            .Where(q => q.CreatorId == creatorId);

        if (IsActive.HasValue)
            query = query.Where(q => q.IsActive == IsActive.Value);
        
        if (questionDifficulty.HasValue)
            query = query.Where(q => q.DifficultyLevel == questionDifficulty.Value);
        
        int totalQuestions = query.Count();

        List<QuestionDto>? QuestionsList = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync()
            .ContinueWith(task => task.Result.Adapt<List<QuestionDto>>());

        return (QuestionsList, totalQuestions);
    }
}