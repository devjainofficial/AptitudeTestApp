using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
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
}