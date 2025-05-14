using TurboNotes.Core.Services;

namespace TurboNotes.Core.Models;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = TimeService.GetCurrentUtcTime();
    public DateTime? Deadline { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}