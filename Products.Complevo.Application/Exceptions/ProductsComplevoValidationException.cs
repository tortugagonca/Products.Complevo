using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace Products.Complevo.Application.Core.Exceptions
{
    public class ProductsComplevoValidationException : Exception
    {
        public ICollection<ValidationFailure> Errors { get; set; }

        public ProductsComplevoValidationException(string message):base(message)
        {

        }
         
        public ProductsComplevoValidationException(ICollection<ValidationFailure> errors)
        {
            Errors = errors;
        }
    }
}
