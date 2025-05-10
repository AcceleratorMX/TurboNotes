using TurboNotes.Core.Models;

namespace TurboNotes.Core.Interfaces;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllAsync();
    Task<Note> GetByIdAsync(int id);
    Task AddAsync(Note note);
    Task UpdateAsync(Note note);
    Task DeleteAsync(int id); 
}