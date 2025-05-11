using Microsoft.AspNetCore.Mvc;
using TurboNotes.Core.Interfaces;
using TurboNotes.Web.Models.ViewModels;

namespace TurboNotes.Web.Components;

public class NavigationMenuViewComponent(ICategoryRepository categoryRepository) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        var filteredCategories = categories.Where(c => c.Id != 1).ToList();
        var categoryIdString = HttpContext.Request.Query["categoryId"].ToString();
        int? selectedCategory = string.IsNullOrEmpty(categoryIdString) ? null : int.Parse(categoryIdString);

        return View(new NavigationMenuViewModel
        {
            Categories = filteredCategories,
            SelectedCategory = selectedCategory
        });
    }
}