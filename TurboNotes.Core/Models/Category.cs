namespace TurboNotes.Core.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Note> Notes { get; set; } = [];
}