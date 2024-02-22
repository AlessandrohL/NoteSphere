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
        Task<List<Notebook>> FindNotebooksAsync(
            NotebooksFilter filter, 
            CancellationToken cancellationToken);
        Task<Notebook?> FindNotebookByIdAsync(
            Guid id,
            bool trackChanges,
            CancellationToken cancellationToken,
            bool ignoreQueryFilter = false);

        Task<bool> IsNotebookExistsAsync(Guid id, CancellationToken cancellationToken);
    }

}