using ShopperAPi.Errors;
using System.Net;
using System.Text.Json;

namespace ShopperAPi.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public CustomExceptionMiddleware(RequestDelegate next ,
            ILogger<CustomExceptionMiddleware>logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                 await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e , e.Message);
                context.Response.ContentType = "application/json";
                 
                var response = _environment.IsDevelopment()
             ? new ApiException((int)context.Response.StatusCode, e.Message, e.StackTrace.ToString())
             : new ApiErrorResponse((int)context.Response.StatusCode);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
