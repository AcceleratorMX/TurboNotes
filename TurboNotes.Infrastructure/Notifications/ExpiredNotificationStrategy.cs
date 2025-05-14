using TurboNotes.Core.Interfaces;

namespace TurboNotes.Infrastructure.Notifications;

public class ExpiredNotificationStrategy : INotificationStrategy
{
    public bool CanHandle(TimeSpan timeUntilDeadline) => timeUntilDeadline <= TimeSpan.Zero;

    public (string Message, string NotificationType) CreateNotification(string noteTitle, TimeSpan timeUntilDeadline)
        => ($"⌛ Deadline passed: {noteTitle}", "expired");
}