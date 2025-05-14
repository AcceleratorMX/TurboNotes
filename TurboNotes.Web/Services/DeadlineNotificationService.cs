using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Services;

namespace TurboNotes.Web.Services;

public class DeadlineNotificationService(
    IServiceScopeFactory scopeFactory,
    NotificationStrategyContext strategyContext)
    : BackgroundService
{
    private readonly Dictionary<int, (DateTime Deadline, string LastNotificationType)> _notifiedNotes = new();
    private readonly Lock _lock = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var noteRepository = scope.ServiceProvider.GetRequiredService<INoteRepository>();
            var notificationSender = scope.ServiceProvider.GetRequiredService<INotificationSender>();

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
                    if (!note.Deadline.HasValue)
                    {
                        continue;
                    }

                    var deadline = note.Deadline.Value;
                    var timeUntilDeadline = deadline - now;

                    var strategy = strategyContext.GetStrategy(timeUntilDeadline);
                    var (message, notificationType) = strategy.CreateNotification(note.Title, timeUntilDeadline);

                    if (_notifiedNotes.TryGetValue(note.Id, out var existing) &&
                        existing.LastNotificationType == notificationType &&
                        existing.Deadline == deadline)
                    {
                        continue;
                    }

                    var notification = new
                    {
                        Id = note.Id,
                        NoteTitle = note.Title,
                        Message = message,
                        Type = notificationType,
                        Deadline = deadline.ToString("o")
                    };

                    notificationSender.SendNotificationAsync(notification, stoppingToken).GetAwaiter().GetResult();
                    _notifiedNotes[note.Id] = (deadline, notificationType);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}