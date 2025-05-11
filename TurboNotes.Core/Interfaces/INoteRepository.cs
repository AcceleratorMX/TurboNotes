using TurboNotes.Core.Models;

namespace TurboNotes.Core.Interfaces;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllAsync(int page, int pageSize);
    Task<Note> GetByIdAsync(int id);
    Task<IEnumerable<Note>> GetByCategoryAsync(int categoryId, int page, int pageSize);
    Task<int> GetTotalCountAsync(int? categoryId);
    Task CreateAsync(Note note);
    Task UpdateAsync(Note note);
    Task DeleteAsync(int id); 
}