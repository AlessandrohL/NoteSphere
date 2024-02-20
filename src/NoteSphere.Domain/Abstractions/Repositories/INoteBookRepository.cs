using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Repositories
{
    public interface INotebookRepository : IRepositoryBase<Notebook, Guid>
    {
        Task<List<Notebook>> FindNotebooksAsync(NotebooksFilter filter);
        Task<Notebook?> FindNotebookById(
            Guid id,
            bool trackChanges,
            bool ignoreQueryFilter = false);
        Task<bool> IsNotebookExistsAsync(Guid applicationUserId);
        //Task<bool> IsNotebookDeletedAsync(Guid applicationUserId, Guid id);

    }

}