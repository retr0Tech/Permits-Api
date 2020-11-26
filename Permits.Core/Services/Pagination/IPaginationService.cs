using Permits.Core.Options;
using System.Linq;

namespace Permits.Core.Services.Pagination
{
    public interface IPaginationService
    {
        IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> entitiesQuery, string where);
        IQueryable<TEntity> Order<TEntity>(IQueryable<TEntity> entitiesQuery, IApiQueryOption paginationInfo);
        IQueryable<TEntity> Pagination<TEntity>(IQueryable<TEntity> entitiesQuery, IApiQueryOption paginationInfo);
        IQueryable<TEntity> SetHeader<TEntity>(IQueryable<TEntity> entitiesQuery, IApiQueryOption paginationInfo);
    }
}
