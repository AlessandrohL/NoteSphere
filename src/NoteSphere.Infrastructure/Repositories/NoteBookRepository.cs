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
            NotebooksFilter filter)
        {
            var queryBase = FindAll(trackChanges: false);
            var filteredQuery = NotebookQuery.Generate(queryBase, filter);

            return await filteredQuery.ToListAsync();
        }

        public async Task<Notebook?> FindNotebookById(
            Guid id,
            bool trackChanges,
            bool ignoreQueryFilter = false)
        {
            var query = FindByCondition(nb => nb.Id == id, trackChanges);

            return ignoreQueryFilter
                ? await query.IgnoreQueryFilters().FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }

        public Task<bool> IsNotebookExistsAsync(Guid id)
        {
            return FindByCondition(nb => nb.Id == id, trackChanges: false)
                .AnyAsync();
        }

    }
}
