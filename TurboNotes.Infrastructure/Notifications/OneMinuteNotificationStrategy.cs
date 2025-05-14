using TurboNotes.Core.Interfaces;

namespace TurboNotes.Infrastructure.Notifications;

public class OneMinuteNotificationStrategy : INotificationStrategy
{
    public bool CanHandle(TimeSpan timeUntilDeadline) => timeUntilDeadline.TotalMinutes <= 1;

    public (string Message, string NotificationType) CreateNotification(string noteTitle, TimeSpan timeUntilDeadline)
        => ($"⚠ Deadline in 1 minute: {noteTitle}", "1min");
}