using Microsoft.AspNetCore.Builder;

namespace module_10.Middleware
{
    public static class HandleExceptionExtension
    {
        public static IApplicationBuilder UseHandleException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandleExceptionMiddleware>();
        }
    }
}
