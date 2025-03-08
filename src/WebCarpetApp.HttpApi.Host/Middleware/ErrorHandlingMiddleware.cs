using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Validation;

namespace WebCarpetApp.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware, ITransientDependency
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IExceptionToErrorInfoConverter _exceptionToErrorInfoConverter;

        public ErrorHandlingMiddleware(
            ILogger<ErrorHandlingMiddleware> logger,
            IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
        {
            _logger = logger;
            _exceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            // Log the exception
            if (exception is BusinessException businessException)
            {
                _logger.LogWarning(businessException, "Business exception occurred: {Message}", businessException.Message);
                context.Response.StatusCode = GetStatusCode(businessException.Code);
            }
            else if (exception is AbpValidationException validationException)
            {
                _logger.LogWarning(validationException, "Validation exception occurred");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception is EntityNotFoundException notFoundException)
            {
                _logger.LogWarning(notFoundException, "Entity not found exception occurred");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                _logger.LogError(exception, "Unhandled exception occurred");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            // Use ABP's error conversion
            var remoteServiceErrorInfo = _exceptionToErrorInfoConverter.Convert(exception, options => 
            {
                options.SendExceptionsDetailsToClients = true;
                options.SendStackTraceToClients = false;
            });

            return context.Response.WriteAsync(JsonSerializer.Serialize(new 
            {
                error = remoteServiceErrorInfo
            }));
        }

        private int GetStatusCode(string code)
        {
            if (code == null)
            {
                return (int)HttpStatusCode.InternalServerError;
            }

            if (code.EndsWith(":00001")) // EntityNotFound
            {
                return (int)HttpStatusCode.NotFound;
            }
            
            if (code.EndsWith(":00002")) // InvalidOperation
            {
                return (int)HttpStatusCode.BadRequest;
            }

            return (int)HttpStatusCode.BadRequest;
        }
    }
} 