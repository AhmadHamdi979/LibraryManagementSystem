using Library.Shared.Resources;
using Library.Shared.Exceptions;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;

namespace Library.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IStringLocalizer<SharedResource> localizer)
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (exception is NotFoundException) statusCode = HttpStatusCode.NotFound;
            if (exception is UnauthorizedException) statusCode = HttpStatusCode.Unauthorized;
            if (exception is BadRequestException) statusCode = HttpStatusCode.BadRequest;

            string messageKey = exception.Message ?? "UnexpectedError";

           
            var localizedMessage = _localizer[messageKey];

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            
            var result = JsonSerializer.Serialize(new { error = localizedMessage.Value });
            return context.Response.WriteAsync(result);
        }
    }
}
