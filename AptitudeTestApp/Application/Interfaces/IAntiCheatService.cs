namespace AptitudeTestApp.Application.Interfaces;
public interface IAntiCheatService
{
    Task LogEventAsync(Guid submissionId, string eventType, string eventDetails);
    Task<bool> ShouldDisqualifyAsync(Guid submissionId);
    Task IncrementTabSwitchAsync(Guid submissionId);
}