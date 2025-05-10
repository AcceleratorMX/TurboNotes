using Microsoft.EntityFrameworkCore;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;
using TurboNotes.Infrastructure.Data;

namespace TurboNotes.Infrastructure.Repositories;

public class NoteRepository(TurboNotesDbContext context) : INoteRepository
{
    public async Task<IEnumerable<Note>> GetAllAsync() =>
        await context.Notes.Include(n => n.Category).ToListAsync();

    public async Task<Note> GetByIdAsync(int id) =>
        await context.Notes
            .Include(n => n.Category)
            .FirstOrDefaultAsync(n => n.Id == id)
        ?? throw new InvalidOperationException($"Note with id {id} not found.");

    public async Task AddAsync(Note note)
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