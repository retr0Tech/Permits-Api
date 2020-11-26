using Microsoft.EntityFrameworkCore;
using Permits.Core.Models;
using Permits.Core.Services.Pagination;
using Permits.Model.Contexts.Permits;
using Permits.Model.Repositories;
using Permits.Model.Repositories.Generic;
using Permits.Model.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Permits.Model.UnitOfWork.Permits
{
    class PermitsUnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, IPermitsDbContext
    {
        public TContext context { get; set; }
        public readonly IServiceProvider _serviceProvider;
        public readonly IPaginationService _paginationService;

        public PermitsUnitOfWork(IServiceProvider serviceProvider, TContext context, IPaginationService paginationService)
        {
            _serviceProvider = serviceProvider;
            _paginationService = paginationService;
            this.context = context;
        }

        public async Task<int> Commit()
        {
            var result = await context.SaveChangesAsync();
            return result;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IBaseRepository<TEntity, TContext> GetRepository<TEntity>() where TEntity : class, IBaseEntity
        {
            return (_serviceProvider.GetService(typeof(TEntity)) ?? new BaseRepository<TEntity, TContext>(this)) as IBaseRepository<TEntity, TContext>;
        }

        public IPaginationService GetPaginationService()
        {
            return _paginationService ?? _serviceProvider.GetService(typeof(PaginationService)) as IPaginationService;
        }
    }
}
