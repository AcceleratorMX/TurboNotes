using TurboNotes.Core.Interfaces;

namespace TurboNotes.Core.Services;

public static class TimeService
{
    public static DateTime GetCurrentUtcTime() => DateTime.UtcNow;
    
    public static DateTime? ToUtc(DateTime? localTime)
    {
        if (!localTime.HasValue)
        {
            return null;
        }

        return DateTime.SpecifyKind(localTime.Value, DateTimeKind.Local).ToUniversalTime();
    }
    
    public static DateTime? ToLocal(DateTime? utcTime)
    {
        if (!utcTime.HasValue)
        {
            return null;
        }
        
        var properUtcTime = DateTime.SpecifyKind(utcTime.Value, DateTimeKind.Utc);
        return properUtcTime.ToLocalTime();
    }
}