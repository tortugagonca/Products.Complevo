using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Products.Complevo.Application.Core.Mappers;

namespace Products.Complevo.Infrastructure.CrossCutting.IoC
{
    static class Mappers
    {
        public static void AddMappers(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new ProductsMapper()));
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
