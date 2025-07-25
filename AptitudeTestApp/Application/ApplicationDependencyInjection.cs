using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Application.Services;
using AptitudeTestApp.Infrastructure.Persistence;
using AptitudeTestApp.Infrastructure.Persistence.Repositories;

namespace AptitudeTestApp.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<ITestSessionService, TestSessionService>();
        services.AddScoped<IAntiCheatService, AntiCheatService>();
        services.AddScoped<IUniversityService, UniversityService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IRepository, Repository<ApplicationDbContext>>();
        services.AddScoped<JavaScriptService>();
        services.AddScoped<DataSeedingService>();

        return services;
    }
}
