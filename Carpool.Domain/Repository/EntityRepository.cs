using Carpool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Carpool.Domain.Repository
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntity, new()
    {
        readonly DbContext _entitiesContext;

        public EntityRepository(DbContext entitiesContext)
        {
            if (entitiesContext == null)
            {
                throw new ArgumentNullException("entitiesContext");
            }
            _entitiesContext = entitiesContext;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entitiesContext.Set<T>();
        }

        public virtual IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entitiesContext.Set<T>();
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T GetSingle(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _entitiesContext.Set<T>().Where(predicate);
        }

        public async virtual Task<PaginatedList<T>> PaginateAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector)
        {
            return await PaginateAsync(pageIndex, pageSize, keySelector, null);
        }

        public async virtual Task<PaginatedList<T>> PaginateAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, 
            Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AllIncluding(includeProperties).OrderBy(keySelector);
            query = (predicate == null)
                ? query
                : query.Where(predicate);
            return await query.ToPaginatedListAsync(pageIndex, pageSize);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            _entitiesContext.Set<T>().Add(entity);
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Upsert(T entity)
        {
            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = entity.Id == 0
                                    ? EntityState.Added
                                    : EntityState.Modified;
        }

        public virtual void DeleteAll(ICollection<T> entities)
        {
            // reverse loop to avoid causing error in shifting list on delete
            for (var i = entities.Count - 1; i >= 0; i--)
            {
                Delete(entities.ElementAt(i));
            }
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual async Task DeleteByIdAsync(int id)
        {
            T entity = await FindBy(e => e.Id == id).SingleAsync();
            Delete(entity);
        }
    }
}
