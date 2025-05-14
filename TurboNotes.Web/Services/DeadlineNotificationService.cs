using Microsoft.AspNetCore.SignalR;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Services;
using TurboNotes.Web.Hubs;

namespace TurboNotes.Web.Services;

public class DeadlineNotificationService(IServiceScopeFactory scopeFactory, IHubContext<NotificationHub> hubContext)
    : BackgroundService
{
    private readonly Dictionary<int, (DateTime Deadline, string LastMessage)> _notifiedNotes = new();
    private readonly Lock _lock = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var noteRepository = scope.ServiceProvider.GetRequiredService<INoteRepository>();

            var notes = await noteRepository.GetAllWithDeadlineAsync();
            var now = TimeService.GetCurrentUtcTime();

            lock (_lock)
            {
                var notesWithDeadline = new HashSet<int>(notes.Select(n => n.Id));
                
                var keysToRemove = _notifiedNotes.Keys.Except(notesWithDeadline).ToList();
                foreach (var key in keysToRemove)
                {
                    _notifiedNotes.Remove(key);
                }
                
                foreach (var note in notes)
                {
                    var deadline = note.Deadline!.Value;
                    var timeUntilDeadline = deadline - now;

                    string message;
                    string notificationType;

                    if (timeUntilDeadline <= TimeSpan.Zero)
                    {
                        message = $"⌛ Deadline passed: {note.Title}";
                        notificationType = "expired";
                    }
                    else if (timeUntilDeadline.TotalMinutes <= 1)
                    {
                        message = $"⚠ Deadline in 1 minute: {note.Title}";
                        notificationType = "1min";
                    }
                    else if (timeUntilDeadline.TotalMinutes <= 60)
                    {
                        message = $"⏳ Deadline in {timeUntilDeadline.TotalMinutes:F0} minutes: {note.Title}";
                        notificationType = "60min";
                    }
                    else if (timeUntilDeadline.TotalHours <= 24)
                    {
                        message = $"📅 Deadline in {timeUntilDeadline.TotalHours:F0} hours: {note.Title}";
                        notificationType = "24h";
                    }
                    else if (timeUntilDeadline.TotalDays <= 7)
                    {
                        message = $"🔜 Deadline in {timeUntilDeadline.TotalDays:F0} days: {note.Title}";
                        notificationType = "7d";
                    }
                    else
                    {
                        message = $"🗓 Deadline in {timeUntilDeadline.TotalDays:F0} days: {note.Title}";
                        notificationType = "30d";
                    }
                    
                    if (_notifiedNotes.TryGetValue(note.Id, out var existing) &&
                        existing.LastMessage == notificationType) continue;
                    
                    var notification = new
                    {
                        Id = note.Id,
                        Message = message,
                        Type = notificationType,
                        Deadline = deadline.ToString("o")
                    };

                    hubContext.Clients.All.SendAsync(
                        "ReceiveNotification",
                        notification,
                        cancellationToken: stoppingToken);

                    _notifiedNotes[note.Id] = (deadline, notificationType);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}