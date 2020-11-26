using Microsoft.Extensions.DependencyInjection;
using Permits.Core.Services.Pagination;
using Permits.Core.Services.Parser;
using Permits.Core.Wrappers;

namespace Permits.Core.IoC
{
    public static class CoreRegistry
    {
        public static void AddCoreRegistry(this IServiceCollection services)
        {
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddScoped<IUrlFilterToDynamicLinqParser, UrlFilterToDynamicLinqParser>();
            services.AddScoped<ICurrentHttpContextWrapper, CurrentHttpContextWrapper>();
        }
    }
}
