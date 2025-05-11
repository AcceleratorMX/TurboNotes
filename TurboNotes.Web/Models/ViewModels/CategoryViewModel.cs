using TurboNotes.Core.Models;

namespace TurboNotes.Web.Models.ViewModels;

public class CategoryViewModel
{
    public IEnumerable<Category> Categories { get; init; } = [];
    public Category Category { get; set; } = null!;
}