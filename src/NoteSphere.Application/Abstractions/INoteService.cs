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
        Task<Result<List<NoteDto>, string>> GetNotesAsync(
            string identityId,
            Guid notebookId,
            NotesFilter request);
    }
}
