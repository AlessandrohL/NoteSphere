using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions.Repositories
{
    public interface IRepositoryBase<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        IQueryable<TEntity> FindAll(bool trackChanges);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges);
        TEntity FindById(TKey id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
