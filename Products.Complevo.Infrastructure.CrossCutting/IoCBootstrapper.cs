using Microsoft.Extensions.DependencyInjection;
using Products.Complevo.Infrastructure.CrossCutting.IoC;

namespace Products.Complevo.Infrastructure.CrossCutting
{
    public static class IoCBootstrapper
    { 
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddValidators();
            services.AddMappers();
        }
    }
}
