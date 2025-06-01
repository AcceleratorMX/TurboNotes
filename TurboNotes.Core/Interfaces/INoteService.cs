using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboNotes.Core.Models;

namespace TurboNotes.Core.Interfaces
{
    public interface INoteService
    {
        Task<List<Note>> GetAllAsync(string userId);
        Task<Note?> GetByIdAsync(int id, string userId);
        Task CreateAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(int id, string userId);
    }
}
