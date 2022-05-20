using FluentValidation.Results;
using Products.Complevo.Application.Core.Exceptions;

namespace Products.Complevo.Application.Core.Extensions
{
    public static class FluentValidationExtensions
    {
        public static void ThrowExceptionIfNotValid(this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ProductsComplevoValidationException(validationResult.Errors);
            }
        }
    }
}
