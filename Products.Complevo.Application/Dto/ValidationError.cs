using System.Collections.Generic;

namespace Products.Complevo.Application.Core.Dto
{
    public class ValidationError
    {
        public string Field { get; set; }
        public string MessageCode { get; set; }

        public string Message { get; set; }

        public object Value { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}
