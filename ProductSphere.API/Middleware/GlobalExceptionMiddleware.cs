using System.Net;
using System.Text.Json;

namespace ProductSphere.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var responce = new
                {
                    StatusCodes = context.Response.StatusCode,
                    Message = "An unexpected error occurred."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(responce));

            }
        }
    }
}
