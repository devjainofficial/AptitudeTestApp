using AptitudeTestApp.Application.Interfaces;
using Radzen;

namespace AptitudeTestApp.Application.Services;

public class ToastService(NotificationService notificationService) : IToastService
{
    private const int Duration = 5000;
    public void Toaster(string summary, string detail,
        NotificationSeverity severity, int Duration = Duration)
    {
        var message = new NotificationMessage
        {
            Severity = severity,
            Summary = summary,
            Detail = detail,
            Duration = Duration
        };

        notificationService.Notify(message);
    }
    public void Info(string summary, string detail, int duration = Duration)
    {
        Toaster(summary, detail, NotificationSeverity.Info, duration);
    }

    public void Success(string summary, string detail, int duration = Duration)
    {
        Toaster(summary, detail, NotificationSeverity.Success, duration);
    }

    public void Warning(string summary, string detail, int duration = Duration)
    {
        Toaster(summary, detail, NotificationSeverity.Warning, duration);
    }

    public void Error(string summary, string detail, int duration = Duration)
    {
        Toaster(summary, detail, NotificationSeverity.Error, duration);
    }
}
