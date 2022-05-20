using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Products.Complevo.Service.Commands.Products;
using Products.Complevo.Service.Commands.Validator;

namespace Products.Complevo.Infrastructure.CrossCutting.IoC
{
    public static class Validators
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<InsertProductCommand>, InsertProductCommandValidator>();
            services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
            services.AddScoped<IValidator<DeleteProductCommand>, DeleteProductCommandValidator>(); 
        }
    }
}
