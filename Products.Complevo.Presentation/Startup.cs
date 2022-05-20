using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.Complevo.Application.Configurations;
using Products.Complevo.Application.Filter;
using Products.Complevo.Infrastructure.CrossCutting;
using Products.Complevo.Infrastructure.Data;
using Products.Complevo.Infrastructure.Data.Extensions;
using Products.Complevo.Service.Commands.Products;
using System;

namespace Products.Complevo.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterDependencies();
            var assemblyCommands = AppDomain.CurrentDomain.Load(typeof(InsertProductCommand).Assembly.GetName().Name); 
            services.AddMediatR(assemblyCommands);
             
            services.AddMvc(); 

            services.AddControllers();
            AddContextConfiguration(services, Configuration);
            services.AddSwaggerConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products.Complevo.Presentation v1"));
            }
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ApplyMigrationsOnDatabase(app);
        } 

        private static void ApplyMigrationsOnDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            using var contexto = serviceScope.ServiceProvider.GetService<ProductsComplevoContext>();

            if (contexto.DatabaseInMemory())
            {
                return;
            }

            if (!contexto.AllMigrationsApplied())
            {
                contexto.Database.Migrate();
            }
        }

        private void AddContextConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var commandTimeout = GetCommandTimeoutInMinutes(configuration);

            services.AddDbContext<ProductsComplevoContext>(builder =>
                builder.UseSqlServer(
                    configuration.GetConnectionString("ProductsComplevoContext"),
                    opts => opts.CommandTimeout(
                        (int)TimeSpan.FromMinutes(commandTimeout).TotalSeconds
                    )
                )
            );
        } 

        private static double GetCommandTimeoutInMinutes(IConfiguration configuration)
        {
            const double commandTimeoutInMinutesDefault = 15;

            double.TryParse(
                configuration.GetSection("ProductsComplevo:CommandTimeoutInMinutes").Value,
                out var commandTimeoutInMinutes
            );

            if (commandTimeoutInMinutes == 0)
            {
                commandTimeoutInMinutes = commandTimeoutInMinutesDefault;
            }

            return commandTimeoutInMinutes;
        } 
    }
}
