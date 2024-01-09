using Domain.Entities;
using Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Repositories
{
    public interface INoteBookRepository : IRepositoryBase<NoteBook, Guid>
    {
        Task<List<NoteBook>> FindNotebooksAsync(NoteBooksFilter request, Guid applicationUserId);
        Task<NoteBook?> FindNotebookById(
            Guid id,
            Guid applicationUserId,
            bool trackChanges,
            bool ignoreQueryFilter = false);
        Task<bool> IsNotebookExistsAsync(Guid applicationUserId, Guid id);

    }

}