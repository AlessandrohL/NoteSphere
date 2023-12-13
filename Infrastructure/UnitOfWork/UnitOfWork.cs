using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public INoteBookRepository NoteBook { get; }

        public INoteRepository Note { get; }

        public ITagRepository Tag { get; }

        public ITodoRepository Todo { get; }

        public IUserProfileRepository UserProfile { get; }


        public UnitOfWork(ApplicationDbContext context,
            INoteBookRepository noteBookRepository,
            INoteRepository noteRepository,
            ITagRepository tagRepository,
            ITodoRepository todoRepository,
            IUserProfileRepository userProfileRepository)
        {
            _context = context;
            NoteBook = noteBookRepository;
            Note = noteRepository;
            Tag= tagRepository;
            Todo = todoRepository;
            UserProfile = userProfileRepository;
        }


        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
