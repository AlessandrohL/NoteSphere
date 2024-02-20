using Application.Common;
using Application.DTOs.Notes;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface INoteService
    {
        Task<List<NoteDto>> GetAllNotesAsync(Guid notebookId, NotesFilter request);
        Task<NoteDto> GetNoteByIdAsync(Guid notebookId, Guid noteId);
        Task<NoteDto> CreateNoteAsync(Guid notebookId, CreateNoteDto noteDto);
    }
}
