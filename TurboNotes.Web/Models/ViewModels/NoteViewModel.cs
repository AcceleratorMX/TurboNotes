using TurboNotes.Core.Models;

namespace TurboNotes.Web.Models.ViewModels;

public class NoteViewModel
{
    public IEnumerable<Note> Notes { get; init; } = [];
    public PagingInfo PagingInfo { get; init; } = new();
    public IEnumerable<Category> Categories { get; set; } = [];
    public int? SelectedCategoryId { get; init; }
    public Note Note { get; set; } = null!;
}