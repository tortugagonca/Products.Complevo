using System.Collections.Generic;
using System.Linq;

namespace Products.Complevo.Application.Core.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsNotNullAndHasItems<T>(this IEnumerable<T> @this)
            => @this != null && @this.Any();
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this)
            => @this == null || !@this.Any();

    }
}
