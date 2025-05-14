using TurboNotes.Core.Interfaces;

namespace TurboNotes.Infrastructure.Notifications;

public class OneDayNotificationStrategy : INotificationStrategy
{
    public bool CanHandle(TimeSpan timeUntilDeadline) => timeUntilDeadline.TotalHours <= 24;

    public (string Message, string NotificationType) CreateNotification(string noteTitle, TimeSpan timeUntilDeadline)
        => ($"📅 Deadline in {timeUntilDeadline.TotalHours:F0} hours: {noteTitle}", "24h");
}