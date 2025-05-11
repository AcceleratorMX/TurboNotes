using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;
using TurboNotes.Web.Models.ViewModels;

namespace TurboNotes.Web.Controllers;

public class NotesController(INoteRepository noteRepository, ICategoryRepository categoryRepository) : Controller
{
    public async Task<IActionResult> Create()
    {
        var model = new NoteViewModel { Note = new Note() };

        ViewBag.Categories = await GetCategoriesSelectList();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategoriesSelectList();
            return View(model);
        }

        var note = new Note
        {
            Title = model.Note.Title,
            Content = model.Note.Content,
            Deadline = model.Note.Deadline,
            CategoryId = model.Note.CategoryId,
            CreatedAt = DateTime.UtcNow
        };
        await noteRepository.CreateAsync(note);
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var note = await noteRepository.GetByIdAsync(id);
        var model = new NoteViewModel { Note = note };

        ViewBag.Categories = await GetCategoriesSelectList();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NoteViewModel model)
    {
        if (id != model.Note.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await GetCategoriesSelectList();
            return View(model);
        }

        var note = await noteRepository.GetByIdAsync(id);

        note.Title = model.Note.Title;
        note.Content = model.Note.Content;
        note.Deadline = model.Note.Deadline;
        note.CategoryId = model.Note.CategoryId;

        await noteRepository.UpdateAsync(note);
        return RedirectToAction(nameof(Index), "Home");
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var note = await noteRepository.GetByIdAsync(id);
        var model = new NoteViewModel { Note = note };
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, NoteViewModel model)
    {
        if (id != model.Note.Id)
        {
            return BadRequest();
        }

        await noteRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index), "Home");
    }

    private async Task<SelectList> GetCategoriesSelectList()
    {
        var categories = await categoryRepository.GetAllAsync();
        return new SelectList(categories, "Id", "Name");
    }
}