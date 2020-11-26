
using Microsoft.Extensions.DependencyInjection;
using Permits.Model.Contexts.Permits;
using Permits.Model.UnitOfWork.Permits;
using Permits.Model.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Permits.Model.IoC
{
    public static class ModelRegistry
    {
        public static void AddModelRegistry(this IServiceCollection services)
        {
            services.AddTransient<IPermitsDbContext, PermitsDbContext>();
            services.AddScoped<IUnitOfWork<PermitsDbContext>, PermitsUnitOfWork<PermitsDbContext>>();
        }
    }
}
