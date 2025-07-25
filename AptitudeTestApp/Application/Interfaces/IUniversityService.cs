using AptitudeTestApp.Data.Models;

namespace AptitudeTestApp.Application.Interfaces;

public interface IUniversityService
{
    Task<int> GetTotalCountAsync();
    Task<List<University>> GetAllUniversitiesAsync();
}
