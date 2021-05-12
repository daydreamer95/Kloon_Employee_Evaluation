using Kloon.EmployeePerformance.Logic.Caches;
using Kloon.EmployeePerformance.Logic.Services.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Kloon.EmployeePerformance.Logic.Common
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomizedService<T>(this IServiceCollection services)
          where T : class, IIdentityService
        {
            services.AddSingleton<CacheProvider>();
            services.AddScoped(typeof(ILogicService<>), typeof(LogicService<>));
            services.AddScoped(typeof(IAuthenLogicService<>), typeof(AuthenLogicService<>));
            services.AddScoped<IIdentityService, T>();

            return services;
        }
    }
}
