using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;
using TurboNotes.Core.Services;
using TurboNotes.Web.Models.ViewModels;

namespace TurboNotes.Web.Controllers;

public class NotesController : Controller
{
    private readonly NoteService _noteService;
    private const int PageSize = 10;

    public NotesController(NoteService noteService)
    {
        _noteService = noteService;
    }

    public async Task<IActionResult> Index(int? categoryId, int page = 1)
    {
        var notes = categoryId.HasValue
            ? await _noteService.GetNotesByCategoryAsync(categoryId.Value, page, PageSize)
            : await _noteService.GetAllNotesAsync(page, PageSize);

        var totalCount = await _noteService.GetTotalNotesCountAsync(categoryId);

        ViewBag.CurrentCategoryId = categoryId;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        return View(notes);
    }

    public async Task<IActionResult> Details(int id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);
        if (note == null)
            return NotFound();

        return View(note);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Note note)
    {
        if (!ModelState.IsValid)
            return View(note);

        await _noteService.CreateNoteAsync(note);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);
        if (note == null)
            return NotFound();

        return View(note);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Note note)
    {
        if (id != note.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(note);

        await _noteService.UpdateNoteAsync(note);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var note = await _noteService.GetNoteByIdAsync(id);
        if (note == null)
            return NotFound();

        return View(note);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _noteService.DeleteNoteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}