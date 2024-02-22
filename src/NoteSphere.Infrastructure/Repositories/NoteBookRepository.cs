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
    public class NotebookRepository : RepositoryBase<Notebook, Guid>, INotebookRepository
    {
        public NotebookRepository(ApplicationDbContext context)
            : base(context)
        { }

        public async Task<List<Notebook>> FindNotebooksAsync(
            NotebooksFilter filter,
            CancellationToken cancellationToken = default)
        {
            var queryBase = FindAll(trackChanges: false);
            var filteredQuery = NotebookQuery.Generate(queryBase, filter);

            return await filteredQuery.ToListAsync(cancellationToken);
        }

        public async Task<Notebook?> FindNotebookByIdAsync(
            Guid id,
            bool trackChanges,
            CancellationToken cancellationToken = default,
            bool ignoreQueryFilter = false)
        {
            var query = FindByCondition(nb => nb.Id == id, trackChanges);

            return ignoreQueryFilter
                ? await query.IgnoreQueryFilters().FirstOrDefaultAsync(cancellationToken)
                : await query.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<bool> IsNotebookExistsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return FindByCondition(nb => nb.Id == id, trackChanges: false)
                .AnyAsync(cancellationToken);
        }

    }
}
