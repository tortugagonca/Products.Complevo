using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Products.Complevo.Application.Configurations.RouteConstraints
{
    public abstract class EnumRouteConstraint<T> : IRouteConstraint where T : Enum
    {
        protected abstract IEnumerable<T> IgnoredValues { get; }

        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection
        )
        {
            var parametro = values[routeKey]?.ToString();

            var valido = Enum.TryParse(typeof(T), parametro, out var resultado);

            if (!valido || !Enum.IsDefined(typeof(T), resultado))
            {
                return false;
            }

            return !IgnoredValues.Any(x => resultado.Equals(x));
        }
    }
}
