using Library.Shared.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Library.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { message = exception.Message });
                    break;

                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { message = exception.Message });
                    break;

                case UnauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { message = exception.Message });
                    break;

                default:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { message = "An unexpected error occurred." });
                    break;
            }


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrWhiteSpace(result))
            {
                result = JsonSerializer.Serialize(new { message = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
