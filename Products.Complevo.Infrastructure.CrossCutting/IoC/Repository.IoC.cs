using Microsoft.Extensions.DependencyInjection;
using Products.Complevo.Application.Core.Interfaces.Repository;
using Products.Complevo.Application.Core.Services.Decorators;
using Products.Complevo.Infrastructure.Data.Repository;
using Products.Complevo.Infrastructure.Data.Repository.Decorators;

namespace Products.Complevo.Infrastructure.CrossCutting.IoC
{
    static class Repository
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IFilterProductsFactory, FilterProductsFactory>();
        }
    }
}
