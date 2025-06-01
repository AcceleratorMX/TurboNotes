using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboNotes.Core.Interfaces;
using TurboNotes.Core.Models;

namespace TurboNotes.Core.Services
{
    public class NoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public Task<IEnumerable<Note>> GetAllNotesAsync(int page, int pageSize) =>
            _noteRepository.GetAllAsync(page, pageSize);

        public Task<IEnumerable<Note>> GetNotesWithDeadlineAsync() =>
            _noteRepository.GetAllWithDeadlineAsync();

        public Task<Note> GetNoteByIdAsync(int id) =>
            _noteRepository.GetByIdAsync(id);

        public Task<IEnumerable<Note>> GetNotesByCategoryAsync(int categoryId, int page, int pageSize) =>
            _noteRepository.GetByCategoryAsync(categoryId, page, pageSize);

        public Task<int> GetTotalNotesCountAsync(int? categoryId) =>
            _noteRepository.GetTotalCountAsync(categoryId);

        public Task CreateNoteAsync(Note note) =>
            _noteRepository.CreateAsync(note);

        public Task UpdateNoteAsync(Note note) =>
            _noteRepository.UpdateAsync(note);

        public Task DeleteNoteAsync(int id) =>
            _noteRepository.DeleteAsync(id);
    }
}
