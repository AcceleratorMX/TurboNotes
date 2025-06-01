using Microsoft.EntityFrameworkCore;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;
using TurboNotes.Infrastructure.Data;

namespace TurboNotes.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly TurboNotesDbContext _context;

    public NoteRepository(TurboNotesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Note>> GetAllAsync(int page, int pageSize) =>
        await _context.Notes
            .Include(n => n.Category)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<Note>> GetAllWithDeadlineAsync() =>
        await _context.Notes
            .Include(n => n.Category)
            .Where(n => n.Deadline.HasValue)
            .ToListAsync();

    public async Task<Note> GetByIdAsync(int id) =>
        await _context.Notes
            .Include(n => n.Category)
            .FirstOrDefaultAsync(n => n.Id == id)
        ?? throw new InvalidOperationException($"Note with id {id} not found.");

    public async Task<IEnumerable<Note>> GetByCategoryAsync(int categoryId, int page, int pageSize) =>
        await _context.Notes
            .Include(n => n.Category)
            .Where(n => n.CategoryId == categoryId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> GetTotalCountAsync(int? categoryId)
    {
        if (categoryId.HasValue)
        {
            return await _context.Notes.CountAsync(n => n.CategoryId == categoryId.Value);
        }
        return await _context.Notes.CountAsync();
    }

    public async Task CreateAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Note note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var note = await GetByIdAsync(id);
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }
}