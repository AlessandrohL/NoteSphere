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
        Task<List<NoteDto>> GetNotesAsync(
            string identityId,
            Guid notebookId,
            NotesFilter request);

        Task<NoteDto> GetNoteAsync(
            string identityId,
            Guid notebookId,
            Guid noteId);

        Task<NoteDto> CreateNoteAsync(
            string identityId,
            Guid notebookId,
            CreateNoteDto noteDto);
    }
}
