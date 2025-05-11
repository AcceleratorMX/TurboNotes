using Microsoft.EntityFrameworkCore;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;
using TurboNotes.Infrastructure.Data;

namespace TurboNotes.Infrastructure.Repositories;

public class NoteRepository(TurboNotesDbContext context) : INoteRepository
{
    public async Task<IEnumerable<Note>> GetAllAsync(int page, int pageSize) =>
        await context.Notes
            .Include(n => n.Category)
            .OrderByDescending(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<Note> GetByIdAsync(int id) =>
        await context.Notes
            .Include(n => n.Category)
            .FirstOrDefaultAsync(n => n.Id == id)
        ?? throw new InvalidOperationException($"Note with id {id} not found.");

    public async Task<IEnumerable<Note>> GetByCategoryAsync(int categoryId, int page, int pageSize)
    {
        return await context.Notes
            .Include(v => v.Category)
            .Where(v => v.CategoryId == categoryId)
            .OrderByDescending(v => v.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<int> GetTotalCountAsync(int? categoryId)
    {
        if (categoryId.HasValue)
        {
            return await context.Notes.CountAsync(v => v.CategoryId == categoryId.Value);
        }

        return await context.Notes.CountAsync();
    }
    
    public async Task CreateAsync(Note note)
    {
        context.Notes.Add(note);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Note note)
    {
        context.Notes.Update(note);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var note = await GetByIdAsync(id);
        context.Notes.Remove(note);
        await context.SaveChangesAsync();
    }
}