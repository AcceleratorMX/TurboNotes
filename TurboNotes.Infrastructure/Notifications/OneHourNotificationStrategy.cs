using TurboNotes.Core.Interfaces;

namespace TurboNotes.Infrastructure.Notifications;

public class OneHourNotificationStrategy : INotificationStrategy
{
    public bool CanHandle(TimeSpan timeUntilDeadline) => timeUntilDeadline.TotalMinutes <= 60;

    public (string Message, string NotificationType) CreateNotification(string noteTitle, TimeSpan timeUntilDeadline)
        => ($"⏳ Deadline in {timeUntilDeadline.TotalMinutes:F0} minutes: {noteTitle}", "60min");
}