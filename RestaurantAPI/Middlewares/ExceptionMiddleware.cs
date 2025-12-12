using System.Net;
using System.Text.Json;

namespace RestaurantAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Cho request đi qua bình thường
                await _next(context);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi ở bất cứ đâu (Service, Repo, Controller), nó sẽ nhảy vào đây
                _logger.LogError(ex, "Lỗi nghiêm trọng: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new { StatusCode = context.Response.StatusCode, Message = ex.Message, StackTrace = ex.StackTrace?.ToString() }
                    : new { StatusCode = context.Response.StatusCode, Message = "Lỗi hệ thống, vui lòng liên hệ Admin.", StackTrace = "" };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}