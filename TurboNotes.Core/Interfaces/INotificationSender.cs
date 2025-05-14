namespace TurboNotes.Core.Interfaces;

public interface INotificationSender
{
    Task SendNotificationAsync(object notification, CancellationToken cancellationToken);
}