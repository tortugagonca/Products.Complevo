using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation; 
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Products.Complevo.Application.Core.Dto;
using Products.Complevo.Application.Core.Exceptions;
using Products.Complevo.Application.Core.Extensions;
using Products.Complevo.Application.Core.Resources; 

namespace Products.Complevo.Application.Filter
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        private readonly string _messagePlaceholder = "messagePlaceholder-";
        public ExceptionHandlingMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionHandlingMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            } 
            catch (ProductsComplevoValidationException validationException)
            {
                await ValidationHandleExceptionAsync(context, validationException).ConfigureAwait(false);
            } 
            catch (AggregateException exception)
            {
                var validationException = exception.InnerExceptions
                    .OfType<ProductsComplevoValidationException>() 
                    .FirstOrDefault();
                if (validationException != null)
                {
                    await ValidationHandleExceptionAsync(context, validationException);
                    return;
                }
                await HandleExceptionAsync(context, exception);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception).ConfigureAwait(false);
            }
        }

        private Task ValidationHandleExceptionAsync(
            HttpContext context,
            ProductsComplevoValidationException exception
        )
        {
            var errors = exception.Errors.Select(x =>
            {
                Dictionary<string, object> parameters = null;

                if (!x.FormattedMessagePlaceholderValues.IsNullOrEmpty())
                {
                    parameters = x.FormattedMessagePlaceholderValues
                        .Where(x => x.Key.Contains(_messagePlaceholder))
                        .ToDictionary(y =>
                            y.Key.Replace(_messagePlaceholder, string.Empty),
                            y => y.Value
                        );
                }

                return new ValidationError
                {
                    Field = x.PropertyName,
                    MessageCode = x.ErrorCode,
                    Message = x.ErrorMessage,
                    Value = x.CustomState ?? x.AttemptedValue,
                    Parameters= parameters
                };
            });

            var validationMessage = new ValidationMessage
            { 
                ValidationErrors = errors
            };

            return DefaultResponse(context, HttpStatusCode.BadRequest, validationMessage);
        }


        private Task HandleExceptionAsync(
            HttpContext context,
            Exception exception
        )
        {
            _logger.LogCritical(exception, exception.Message); 
            return DefaultResponse(context, HttpStatusCode.InternalServerError, ProductsComplevoResource.InternalServerError, null, null, exception);
        }

        private Task DefaultResponse(
            HttpContext context,
            HttpStatusCode httpStatusCode,
            string mesage,
            string mesageCode,
            Dictionary<string, object> parameters,
            Exception exception
        )
        {
            var content = new
            {
                httpStatusCode,
                mesage,
                mesageCode,
                parameters,
                exception
            };

            return DefaultResponse(context, httpStatusCode, content);
        }


        private Task DefaultResponse(
            HttpContext context,
            HttpStatusCode httpStatusCode,
            object content
        )
        {
            var jsonSerializerSettings = GetDefaultJsonConfiguration();

            var result = JsonConvert.SerializeObject(content, jsonSerializerSettings);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)httpStatusCode;

            return context.Response.WriteAsync(result);
        }


        private JsonSerializerSettings GetDefaultJsonConfiguration()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true,
                    }
                }
            };
        }
    }
}
