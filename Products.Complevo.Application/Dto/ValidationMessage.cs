using System.Collections.Generic;

namespace Products.Complevo.Application.Core.Dto
{
    public class ValidationMessage
    {
        public string Message { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}
