using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public class UniversityService(IRepository Repo) 
    : EntityService<UniversityDto, University>(Repo), IUniversityService
{
    public async Task<List<UniversityDto>> GetAllActiveUniversity(Guid creatorId)
    {
        if (creatorId == Guid.Empty)
            throw new ArgumentException("Creator ID cannot be empty.", nameof(creatorId));

        List<University>? universities = await Repo.GetQueryable<University>()
            .Where(u => u.CreatorId == creatorId && u.IsActive)
            .ToListAsync();

        return universities.Adapt<List<UniversityDto>>();
    }


    public async Task ToggleActivateUniversityAsync(Guid id)
    {
        University? entity = await Repo.GetByIdAsync<University>(id);
        
        if (entity is null) throw new InvalidOperationException("University not found");

        entity.IsActive = !entity.IsActive;
        await Repo.UpdateAsync(entity);
    }
}
