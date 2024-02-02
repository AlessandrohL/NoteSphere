using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public INotebookRepository Notebook { get; }
        public INoteRepository Note { get; }
        public ITagRepository Tag { get; }
        public ITodoRepository Todo { get; }
        public IApplicationUserRepository ApplicationUser { get; }


        public UnitOfWork(ApplicationDbContext context,
            INotebookRepository noteBookRepository,
            INoteRepository noteRepository,
            ITagRepository tagRepository,
            ITodoRepository todoRepository,
            IApplicationUserRepository applicationUserRepository)
        {
            _context = context;
            Notebook = noteBookRepository;
            Note = noteRepository;
            Tag = tagRepository;
            Todo = todoRepository;
            ApplicationUser = applicationUserRepository;
        }


        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}
