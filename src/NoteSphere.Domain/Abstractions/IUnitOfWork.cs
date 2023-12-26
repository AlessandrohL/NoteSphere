using Domain.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IUnitOfWork
    {
        INoteBookRepository NoteBook { get; }
        INoteRepository Note { get; }
        ITagRepository Tag { get; }
        ITodoRepository Todo { get; }
        IApplicationUserRepository ApplicationUser { get; }
        Task<int> SaveChangesAsync();
    }
}
