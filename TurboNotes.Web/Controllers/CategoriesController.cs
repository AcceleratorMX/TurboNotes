using Microsoft.AspNetCore.Mvc;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;
using TurboNotes.Web.Models.ViewModels;

namespace TurboNotes.Web.Controllers;

public class CategoriesController(ICategoryRepository categoryRepository) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Categories()
    {
        var categories = await categoryRepository.GetAllAsync();
        var filteredCategories = categories.Where(c => c.Id != 1);
        return View(new CategoryViewModel { Categories = filteredCategories });
    }

    [HttpGet]
    public IActionResult Create() => View(nameof(Categories));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Categories));
        }

        var category = new Category { Name = model.Category.Name };

        await categoryRepository.CreateAsync(category);
        return RedirectToAction(nameof(Categories));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        var model = new CategoryViewModel
        {
            Category = category
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel model)
    {
        if (id != model.Category.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = await categoryRepository.GetByIdAsync(id);
        category.Name = model.Category.Name;

        await categoryRepository.UpdateAsync(category);
        return RedirectToAction(nameof(Categories));
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        var model = new CategoryViewModel { Category = category };
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CategoryViewModel model)
    {
        if (model.Category.Id != id)
        {
            return BadRequest();
        }

        await categoryRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Categories));
    }
}