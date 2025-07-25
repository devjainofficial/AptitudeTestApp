using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public class UniversityService(IRepository Repo) : IUniversityService
{
    public async Task<List<University>> GetAllUniversitiesAsync()
    {
        return await Repo.GetQueryable<University>()
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    public Task<int> GetTotalCountAsync()
    {
        return Repo.GetQueryable<University>().CountAsync();
    }
}
