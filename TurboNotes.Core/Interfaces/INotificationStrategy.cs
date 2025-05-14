namespace TurboNotes.Core.Interfaces;

public interface INotificationStrategy
{
    bool CanHandle(TimeSpan timeUntilDeadline);
    (string Message, string NotificationType) CreateNotification(string noteTitle, TimeSpan timeUntilDeadline);
}