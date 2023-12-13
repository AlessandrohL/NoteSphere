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
    public class TodoRepository : RepositoryBase<Todo, int>, ITodoRepository
    {

        public TodoRepository(ApplicationDbContext context)
            : base(context)
        { }
    }
}
