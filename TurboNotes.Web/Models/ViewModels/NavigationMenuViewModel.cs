using TurboNotes.Core.Models;

namespace TurboNotes.Web.Models.ViewModels;

public class NavigationMenuViewModel
{
    public IEnumerable<Category> Categories { get; init; } = null!;
    public int? SelectedCategory { get; init; }
}