using AptitudeTestApp.Application.DTOs;

namespace AptitudeTestApp.Application.Interfaces;

public interface IUniversityService : IEntityService<UniversityDto, Guid>
{
    Task ToggleActivateUniversityAsync(Guid id);
    Task<List<UniversityDto>> GetAllActiveUniversity(Guid creatorId);
    Task<(List<UniversityDto> universityList, int totalUniversities)> GetUniversitiesByFiltersAsync
    (
        Guid creatorId,
        int skip,
        int take,
        bool? IsActive
    );
}
