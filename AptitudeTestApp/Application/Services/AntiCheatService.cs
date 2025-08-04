using AptitudeTestApp.Application.Interfaces;
using AptitudeTestApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTestApp.Application.Services;

public class AntiCheatService(IRepository Repo) : IAntiCheatService
{
    public async Task LogEventAsync(Guid submissionId, string eventType, string eventDetails)
    {
        AntiCheatLog log = new ()
        {
            SubmissionId = submissionId,
            EventType = eventType,
            EventDetails = eventDetails,
            Timestamp = DateTime.Now
        };

        await Repo.AddAsync<AntiCheatLog>(log);
    }

    public async Task<bool> ShouldDisqualifyAsync(Guid submissionId)
    {
        StudentSubmission? submission = await Repo.GetQueryable<StudentSubmission>()
            .Include(s => s.TestSession)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null) return false;

        return submission.TabSwitchCount >= submission.TestSession.MaxTabSwitches;
    }

    public async Task IncrementTabSwitchAsync(Guid submissionId)
    {
        StudentSubmission? submission = await Repo.GetQueryable<StudentSubmission>()
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission != null)
        {
            submission.TabSwitchCount++;
            await Repo.SaveChangesAsync();
        }
    }
}
