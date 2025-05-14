using Microsoft.AspNetCore.Mvc;
using TurboNotes.Core.Interfaces;
using TurboNotes.Web.Models;
using TurboNotes.Web.Models.ViewModels;

namespace TurboNotes.Web.Controllers;

public class HomeController(INoteRepository noteRepository, ICategoryRepository categoryRepository) : Controller
{
    private const int PageSize = 12;

    public async Task<IActionResult> Index(int? categoryId, int page = 1)
    {
        var categories = await categoryRepository.GetAllAsync();
        var totalItems = await noteRepository.GetTotalCountAsync(categoryId);
        var notes = (categoryId.HasValue
                ? await noteRepository.GetByCategoryAsync(categoryId.Value, page, PageSize)
                : await noteRepository.GetAllAsync(page, PageSize))
            .ToList();
        
        foreach (var note in notes)
        {
            if (note.Deadline.HasValue)
            {
                note.Deadline = note.Deadline.Value.ToLocalTime();
            }
        }
        
        foreach (var note in notes)
        {
            if (note.Deadline.HasValue)
            {
                note.Deadline = note.Deadline.Value.ToLocalTime();
            }
        }

        var viewModel = new NoteViewModel
        {
            Notes = notes,
            Categories = categories,
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = totalItems
            }
        };

        return View(viewModel);
    }
}