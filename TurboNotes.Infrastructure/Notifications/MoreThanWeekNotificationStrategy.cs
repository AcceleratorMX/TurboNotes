using TurboNotes.Core.Interfaces;

namespace TurboNotes.Infrastructure.Notifications;

public class MoreThanWeekNotificationStrategy : INotificationStrategy
{
    public bool CanHandle(TimeSpan timeUntilDeadline)
        => timeUntilDeadline.TotalDays > 7;

    public (string Message, string NotificationType) CreateNotification(string noteTitle, TimeSpan timeUntilDeadline)
        => ($"🗓 Deadline in {timeUntilDeadline.TotalDays:F0} days: {noteTitle}", "30d");
}