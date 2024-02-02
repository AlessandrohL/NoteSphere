using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity, TKey>
        : IRepositoryBase<TEntity, TKey> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException($"{nameof(context)} is null");
        }

        public IQueryable<TEntity> FindAll(bool trackChanges) => trackChanges
            ? _context.Set<TEntity>()
            : _context.Set<TEntity>().AsNoTracking();

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges)
            => trackChanges
            ? _context.Set<TEntity>().Where(expression)
            : _context.Set<TEntity>().Where(expression).AsNoTracking();

        public void Create(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
    }
}
