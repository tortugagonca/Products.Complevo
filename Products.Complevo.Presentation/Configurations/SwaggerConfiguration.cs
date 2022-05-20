using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Products.Complevo.Application.Configurations
{
    public static class SwaggerConfiguration
    {  

        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            { 
                options.ResolveConflictingActions(apiDescs => apiDescs.First());
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Products Service", Version = "v1" });
                 

                options.OrderActionsBy(description => description.HttpMethod);
                options.DescribeAllParametersInCamelCase(); 
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                var xmlPathAssemblyCore = Path.Combine(AppContext.BaseDirectory, $"{AppDomain.CurrentDomain.Load("Products.Complevo.Application.Core").GetName().Name}.xml");
                options.CustomSchemaIds(x => x.FullName);

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                if (File.Exists(xmlPathAssemblyCore))
                {
                    options.IncludeXmlComments(xmlPathAssemblyCore);
                }
            })
            .AddSwaggerGenNewtonsoftSupport();
        }
    }
}
