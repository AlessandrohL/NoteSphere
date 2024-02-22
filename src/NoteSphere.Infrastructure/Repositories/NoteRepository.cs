using Application.Querys;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NoteRepository : RepositoryBase<Note, Guid>, INoteRepository
    {
        public NoteRepository(ApplicationDbContext context)
            : base(context)
        { }

        public Task<List<Note>> FindNotesAsync(
            NotesFilter request,
            Guid notebookId,
            CancellationToken cancellationToken = default)
        {
            var queryBase = FindAll(trackChanges: false)
                .Where(n => n.NotebookId == notebookId);

            var filteredQuery = NotesQuery.Generate(queryBase, request);

            return filteredQuery.ToListAsync(cancellationToken);
        }

        public async Task<Note?> FindNoteByIdAsync(
            Guid id,
            Guid notebookId,
            bool trackChanges,
            bool ignoreQueryFilter = false,
            CancellationToken cancellationToken = default)
        {
            var query = FindByCondition(n =>
                (n.NotebookId == notebookId && n.Id == id),
                trackChanges);

            return ignoreQueryFilter
                ? await query.IgnoreQueryFilters().FirstOrDefaultAsync(cancellationToken)
                : await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
