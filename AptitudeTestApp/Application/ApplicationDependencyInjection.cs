using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Application.Mappings;
using AptitudeTestApp.Application.Services;
using AptitudeTestApp.Infrastructure.Persistence;
using AptitudeTestApp.Infrastructure.Persistence.Repositories;
using Radzen;

namespace AptitudeTestApp.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<ITestSessionService, TestSessionService>();
        services.AddScoped<IAntiCheatService, AntiCheatService>();
        services.AddScoped<IUniversityService, UniversityService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IStudentSubmissionService, StudentSubmissionService>();
        services.AddScoped<IRepository, Repository<ApplicationDbContext>>();
        services.AddScoped<NotificationService>();
        services.AddScoped<IToastService, ToastService>();
        services.AddScoped<JavaScriptService>();
        services.AddScoped<DataSeedingService>();

        MapsterConfig.RegisterMappings();

        return services;
    }
}
