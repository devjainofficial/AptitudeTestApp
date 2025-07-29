using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Shared.Enums;

namespace AptitudeTestApp.Application.Interfaces;

public interface IQuestionService : IEntityService<QuestionDto, Guid>
{
    Task<List<QuestionCategoryDto>> GetAllActiveCategoriesAsync();
}
