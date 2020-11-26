using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Permits.Core.Options;
using Permits.Model.Entities.LogEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Permits.Model.Repositories.Generic
{
    public interface IBaseRepository<T, TContext> : IDisposable where T : class where TContext : DbContext
    {

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> predicate = null);
        IQueryable<T> GetAllByIds(IList<int> ids, Expression<Func<T, bool>> predicate = null);

        T FirstAsNoTracking(Expression<Func<T, bool>> predicate = null);
        T First(Expression<Func<T, bool>> predicate = null);
        T GetById(int id);
        T GetByIdAsNoTracking(int id);

        EntityEntry<T> Add(T entity);
        IList<EntityEntry<T>> Add(params T[] entities);
        IList<EntityEntry<T>> Add(IEnumerable<T> entities);

        EntityEntry<T> Delete(T entity);
        EntityEntry<T> Delete(int id);
        IList<EntityEntry<T>> Delete(params T[] entities);
        IList<EntityEntry<T>> Delete(IEnumerable<T> entities);

        EntityEntry<T> Update(T entity);
        IList<EntityEntry<T>> Update(params T[] entities);
        IList<EntityEntry<T>> Update(IEnumerable<T> entities);

        void SetEntityState(EntityEntry<T> entityEntry, EntityState state);

        void Detached(T entity);

        IQueryable<T> ApplyApiQueryOption(IApiQueryOption queryOption, IQueryable<T> queryResult);
        IQueryable<T> ApplyFilter(string where, IQueryable<T> queryResult);

        void Log<K>(EntityEntry<T> entityEntry, int? parentEntityId = null, int? relatedEntityId = null) where K : class, ILog;
    }
}
