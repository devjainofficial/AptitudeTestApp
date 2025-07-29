using AptitudeTestApp.Application.DTOs;

namespace AptitudeTestApp.Application.Interfaces;

public interface IUniversityService : IEntityService<UniversityDto, Guid>
{
    Task ToggleActivateUniversityAsync(Guid id);
    Task<List<UniversityDto>> GetAllActiveUniversity(Guid creatorId);
}
