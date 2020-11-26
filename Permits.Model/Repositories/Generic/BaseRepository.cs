using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Permits.Core.Models;
using Permits.Core.Options;
using Permits.Core.Services.Pagination;
using Permits.Model.Contexts.Permits;
using Permits.Model.Entities.LogEntities.Base;
using Permits.Model.Repositories.Generic;
using Permits.Model.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Permits.Model.Extensions;
using Permits.Core.Extensions;

namespace Permits.Model.Repositories
{
    public class BaseRepository<T, TContext> : IBaseRepository<T, TContext> where T : class, IBaseEntity where TContext : DbContext, IPermitsDbContext
    {
        private readonly IUnitOfWork<TContext> _uow;
        private readonly TContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IPaginationService _paginationService;
        public BaseRepository(IUnitOfWork<TContext> uow)
        {
            _uow = uow;
            _context = _uow.context;
            _dbSet = _context.Set<T>();
            _paginationService = _uow.GetPaginationService();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> list = _dbSet.AsQueryable();

            if (predicate is null)
                return list;

            return list.Where(predicate);
        }
        public IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> list = _dbSet.AsQueryable().AsNoTracking();

            if (predicate is null)
                return list;

            return list.Where(predicate);
        }
        public IQueryable<T> GetAllByIds(IList<int> ids, Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> list = _dbSet.AsQueryable().Where(x => ids.Contains(x.Id));

            if (predicate is null)
                return list;

            return list.Where(predicate);
        }

        public T First(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> list = _dbSet.AsQueryable();

            return list.FirstOrDefault();
        }
        public T FirstAsNoTracking(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> list = _dbSet.AsNoTracking().AsQueryable();

            return list.FirstOrDefault();
        }
        public virtual T GetById(int id)
        {
            IQueryable<T> list = _dbSet.AsQueryable();

            return list.FirstOrDefault(x => x.Id == id);
        }
        public virtual T GetByIdAsNoTracking(int id)
        {
            IQueryable<T> list = _dbSet.AsNoTracking().AsQueryable();

            return list.FirstOrDefault(x => x.Id == id);
        }

        public virtual EntityEntry<T> Add(T entity)
        {
            _dbSet.Add(entity);
            var entry = _context.Entry(entity);
            SetEntityState(entry, EntityState.Added);
            return entry;
        }
        public virtual IList<EntityEntry<T>> Add(params T[] entities)
        {
            _dbSet.AddRange(entities);
            var entries = new List<EntityEntry<T>>();
            foreach (T entity in entities)
            {
                var entry = _context.Entry(entity);
                SetEntityState(entry, EntityState.Added);
                entries.Add(entry);
            }
            return entries;
        }
        public virtual IList<EntityEntry<T>> Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            var entries = new List<EntityEntry<T>>();
            foreach (T entity in entities)
            {
                var entry = _context.Entry(entity);
                SetEntityState(entry, EntityState.Added);
                entries.Add(entry);
            }
            return entries;
        }

        public virtual EntityEntry<T> Delete(T entity)
        {
            _dbSet.Remove(entity);
            var entry = _context.Entry(entity);
            SetEntityState(entry, EntityState.Deleted);
            return entry;
        }
        public virtual EntityEntry<T> Delete(int id)
        {
            T entity = GetById(id);
            _dbSet.Remove(entity);
            var entry = _context.Entry(entity);
            SetEntityState(entry, EntityState.Deleted);
            return entry;
        }
        public virtual IList<EntityEntry<T>> Delete(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
            var entries = new List<EntityEntry<T>>();
            foreach (T entity in entities)
            {
                var entry = _context.Entry(entity);
                SetEntityState(entry, EntityState.Deleted);
                entries.Add(entry);
            }
            return entries;
        }
        public virtual IList<EntityEntry<T>> Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            var entries = new List<EntityEntry<T>>();
            foreach (T entity in entities)
            {
                var entry = _context.Entry(entity);
                SetEntityState(entry, EntityState.Deleted);
                entries.Add(entry);
            }
            return entries;
        }

        public virtual EntityEntry<T> Update(T entity)
        {
            _dbSet.Attach(entity);
            var entry = _context.Entry(entity);
            SetEntityState(entry, EntityState.Modified);
            return entry;
        }
        public virtual IList<EntityEntry<T>> Update(params T[] entities)
        {
            var entries = new List<EntityEntry<T>>();
            foreach (T entity in entities)
            {
                _dbSet.Attach(entity);
                var entry = _context.Entry(entity);
                SetEntityState(entry, EntityState.Modified);
                entries.Add(entry);
            }
            return entries;
        }
        public virtual IList<EntityEntry<T>> Update(IEnumerable<T> entities)
        {
            var entries = new List<EntityEntry<T>>();
            foreach (T entity in entities)
            {
                _dbSet.Attach(entity);
                var entry = _context.Entry(entity);
                SetEntityState(entry, EntityState.Modified);
                entries.Add(entry);
            }
            return entries;
        }

        public void SetEntityState(EntityEntry<T> entityEntry, EntityState state)
        {
            entityEntry.State = state;
        }

        public virtual void Detached(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IQueryable<T> ApplyApiQueryOption(IApiQueryOption queryOption, IQueryable<T> queryResult)
        {
            if (_paginationService != null)
            {
                queryResult = ApplyFilter(queryOption.Where, queryResult);
                queryResult = _paginationService.Order(queryResult, queryOption);
                queryResult = _paginationService.SetHeader(queryResult, queryOption);
                queryResult = _paginationService.Pagination(queryResult, queryOption);
            }

            return queryResult;
        }

        public IQueryable<T> ApplyFilter(string where, IQueryable<T> queryResult)
        {
            if (_paginationService != null)
            {
                queryResult = _paginationService.Filter(queryResult, where);
            }

            return queryResult;
        }

        public void Log<K>(EntityEntry<T> entityEntry, int? parentEntityId = null, int? relatedEntityId = null) where K : class, ILog
        {
            if (entityEntry.Entity.Deleted)
            {
                entityEntry.State = EntityState.Deleted;
            }
            DbSet<K> _loggerDbSet = _context.Set<K>();
            if (entityEntry.State == EntityState.Modified)
            {
                var entryProperties = entityEntry.Properties.Where(x =>
                    x.Metadata.Name != nameof(entityEntry.Entity.CreatedBy) &&
                    x.Metadata.Name != nameof(entityEntry.Entity.CreatedDate) &&
                    x.Metadata.Name != nameof(entityEntry.Entity.UpdatedBy) &&
                    x.Metadata.Name != nameof(entityEntry.Entity.UpdatedDate));
                foreach (var entry in entryProperties)
                {
                    var propertyName = entry.Metadata.Name;
                    var originalValue = entityEntry.GetDatabaseValues().GetValue<object>(propertyName)?.ToString();
                    var currentValue = entry.CurrentValue?.ToString();
                    if (originalValue != currentValue)
                    {
                        Type type = entry.EntityEntry.Metadata.FindProperty(propertyName).ClrType;
                        Type underlyingType = Nullable.GetUnderlyingType(type);
                        bool isEnum = underlyingType?.IsEnum ?? false;

                        string mappedOriginalValue = originalValue;
                        string mappedCurrentValue = currentValue;


                        if (entry.Metadata.IsForeignKey())
                        {
                            var entityFullName = entityEntry.Entity.GetType().FullName;
                            var relatedEntityType = _context.Model.GetEntityTypes().FirstOrDefault(x => x.Name == entityFullName)
                                .GetNavigations().FirstOrDefault(x => x.ForeignKey.Properties.FirstOrDefault().Name == propertyName).ClrType;
                            if (relatedEntityType != null)
                            {
                                var relatedEntityDbSet = (IQueryable<IBaseEntity>)_context.GetType().GetMethod("Set").MakeGenericMethod(relatedEntityType).Invoke(_context, null);
                                var originalRelatedEntity = (relatedEntityDbSet.FirstOrDefault(X => X.Id == Convert.ToInt32(originalValue)) as object);
                                var currentRelatedEntity = (relatedEntityDbSet.FirstOrDefault(X => X.Id == Convert.ToInt32(currentValue)) as object);
                                if (originalRelatedEntity?.GetType().GetProperty("Name") != null)
                                {
                                    mappedOriginalValue = (originalRelatedEntity as dynamic).Name;
                                }
                                else if (originalRelatedEntity?.GetType().GetProperty("DisplayName") != null)
                                {
                                    mappedOriginalValue = (originalRelatedEntity as dynamic).DisplayName;
                                }

                                if (currentRelatedEntity?.GetType().GetProperty("Name") != null)
                                {
                                    mappedCurrentValue = (currentRelatedEntity as dynamic).Name;
                                }
                                else if (currentRelatedEntity?.GetType().GetProperty("DisplayName") != null)
                                {
                                    mappedCurrentValue = (currentRelatedEntity as dynamic).DisplayName;
                                }
                            }
                        }

                        K log = Activator.CreateInstance<K>();
                        log.EntityId = entityEntry.Entity.Id;
                        log.EntityState = entityEntry.State;
                        log.EntityName = entityEntry.Entity.GetType().GetDescription();
                        log.PropertyName = entry.Metadata.PropertyInfo.GetDescription();
                        log.OriginalValue = originalValue;
                        log.CurrentValue = currentValue;
                        log.MappedOriginalValue = mappedOriginalValue;
                        log.MappedCurrentValue = mappedCurrentValue;
                        log.ParentEntityId = parentEntityId;
                        log.RelatedEntityId = relatedEntityId;
                        _loggerDbSet.Add(log);
                    }
                }
            }
            else
            {
                K log = Activator.CreateInstance<K>();
                log.EntityId = entityEntry.Entity.Id;
                log.EntityState = entityEntry.State;
                log.EntityName = entityEntry.Entity.GetType().GetDescription();
                log.PropertyName = null;
                log.OriginalValue = null;
                log.CurrentValue = null;
                log.MappedOriginalValue = null;
                log.MappedCurrentValue = null;
                log.ParentEntityId = parentEntityId;
                log.RelatedEntityId = relatedEntityId;
                _loggerDbSet.Add(log);
            }
        }
    }
}
