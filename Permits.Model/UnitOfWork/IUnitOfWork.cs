using Microsoft.EntityFrameworkCore;
using Permits.Core.Models;
using Permits.Core.Services.Pagination;
using Permits.Model.Contexts.Permits;
using Permits.Model.Repositories.Generic;
using System;
using System.Threading.Tasks;

namespace Permits.Model.UnitOfWorks
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext, IPermitsDbContext
    {
        IBaseRepository<TEntity, TContext> GetRepository<TEntity>() where TEntity : class, IBaseEntity;
        IPaginationService GetPaginationService();
        Task<int> Commit();
        TContext context { get; }
    }
}
