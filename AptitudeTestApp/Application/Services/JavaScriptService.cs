using AptitudeTestApp.Data.Models;
using AptitudeTestApp.Shared.Dialogs;
using AptitudeTestApp.Shared.Enums;
using global::AptitudeTestApp.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using Radzen;

namespace AptitudeTestApp.Application.Services;

public class JavaScriptService(IJSRuntime jsRuntime)
{
    [JSInvokable("LogAntiCheatEvent")]
    public static async Task LogAntiCheatEvent(Guid submissionId, string eventType, string eventDetails)
    {
        var serviceProvider = ServiceProviderAccessor.ServiceProvider;

        using var scope = serviceProvider?.CreateScope();
        var antiCheatService = scope?.ServiceProvider.GetService<IAntiCheatService>();

        if (antiCheatService != null)
        {
            await antiCheatService.LogEventAsync(submissionId, eventType, eventDetails);
        }
    }

    [JSInvokable("AutoSubmitTest")]
    public static async Task AutoSubmitTest(Guid submissionId, string reason)
    {
        var serviceProvider = ServiceProviderAccessor.ServiceProvider;

        using var scope = serviceProvider?.CreateScope();
        var studentSessionService = scope?.ServiceProvider.GetService<IStudentSubmissionService>();

        if (studentSessionService is not null)
        {
            await studentSessionService.SubmitTestAsync(submissionId, reason);
        }
    }


    public async Task InitializeAntiCheat(Guid submissionId, int maxTabSwitches)
    {
       await jsRuntime.InvokeVoidAsync("startAntiCheatSystem", submissionId, maxTabSwitches);
    }

    public async Task CleanupAntiCheat()
    {
        await jsRuntime.InvokeVoidAsync("antiCheatSystem.cleanup");
    }
}

// Helper class to access service provider in static methods
public static class ServiceProviderAccessor
{
    public static IServiceProvider? ServiceProvider { get; set; }
}