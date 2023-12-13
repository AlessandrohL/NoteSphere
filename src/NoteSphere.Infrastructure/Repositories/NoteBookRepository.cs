using Domain.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NoteBookRepository : RepositoryBase<NoteBook, Guid>, INoteBookRepository
    {
        public NoteBookRepository(ApplicationDbContext context)
            : base(context)
        { }
    }
}
