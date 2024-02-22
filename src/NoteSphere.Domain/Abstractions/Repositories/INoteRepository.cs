using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Repositories
{
    public interface INoteRepository : IRepositoryBase<Note, Guid>
    {
        Task<List<Note>> FindNotesAsync(
            NotesFilter request,
            Guid notebookId,
            CancellationToken cancellationToken);
        Task<Note?> FindNoteByIdAsync(
            Guid id,
            Guid notebookId,
            bool trackChanges,
            bool ignoreQueryFilter,
            CancellationToken cancellationToken);
    }
}
