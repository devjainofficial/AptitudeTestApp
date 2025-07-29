using AptitudeTestApp.Application.DTOs;
using AptitudeTestApp.Data.Models;
using Mapster;

namespace AptitudeTestApp.Application.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<BaseEntity<Guid>, BaseDto<Guid>>.NewConfig();
        TypeAdapterConfig<BaseDto<Guid>, BaseEntity<Guid>>.NewConfig();

        TypeAdapterConfig<University, UniversityDto>.NewConfig();
        TypeAdapterConfig<UniversityDto, University>
            .NewConfig()
            .Ignore(dest => dest.CreatedAt);
    }
}