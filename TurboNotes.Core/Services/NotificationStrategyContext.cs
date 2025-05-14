using TurboNotes.Core.Interfaces;

namespace TurboNotes.Core.Services;

public class NotificationStrategyContext(IEnumerable<INotificationStrategy> strategies)
{
    public INotificationStrategy GetStrategy(TimeSpan timeUntilDeadline)
    {
        return strategies.FirstOrDefault(s => s.CanHandle(timeUntilDeadline))
               ?? throw new InvalidOperationException("No suitable notification strategy found.");
    }
}