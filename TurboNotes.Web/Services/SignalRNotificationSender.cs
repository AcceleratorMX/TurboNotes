using Microsoft.AspNetCore.SignalR;
using TurboNotes.Core.Interfaces;
using TurboNotes.Web.Hubs;

namespace TurboNotes.Web.Services;

public class SignalRNotificationSender(IHubContext<NotificationHub> hubContext) : INotificationSender
{
    public Task SendNotificationAsync(object notification, CancellationToken cancellationToken)
    {
        return hubContext.Clients.All.SendAsync("ReceiveNotification", notification, cancellationToken);
    }
}