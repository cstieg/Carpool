using Carpool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Carpool.Domain.Repository
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> All { get; }
        IQueryable<T> GetAll();
        T GetSingle(int id);
        Task<T> GetSingleAsync(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<PaginatedList<T>> PaginateAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector);
        Task<PaginatedList<T>> PaginateAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, 
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);
        void Edit(T entity);
        void Upsert(T entity);
        void Delete(T entity);
        void DeleteAll(ICollection<T> entities);
        Task DeleteByIdAsync(int id);
    }
}
