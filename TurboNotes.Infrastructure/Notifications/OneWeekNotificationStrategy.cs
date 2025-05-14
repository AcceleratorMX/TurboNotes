using TurboNotes.Core.Interfaces;

namespace TurboNotes.Infrastructure.Notifications;

public class OneWeekNotificationStrategy : INotificationStrategy
{
    public bool CanHandle(TimeSpan timeUntilDeadline) => timeUntilDeadline.TotalDays <= 7;

    public (string Message, string NotificationType) CreateNotification(string noteTitle, TimeSpan timeUntilDeadline)
        => ($"🔜 Deadline in {timeUntilDeadline.TotalDays:F0} days: {noteTitle}", "7d");
}