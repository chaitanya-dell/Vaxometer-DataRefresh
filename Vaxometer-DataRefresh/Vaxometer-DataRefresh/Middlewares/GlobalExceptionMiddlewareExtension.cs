using Microsoft.AspNetCore.Builder;

namespace Vaxometer_DataRefresh.Middlewares
{
  
    public static class GlobalExceptionMiddlewareExtension
    {
        public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
