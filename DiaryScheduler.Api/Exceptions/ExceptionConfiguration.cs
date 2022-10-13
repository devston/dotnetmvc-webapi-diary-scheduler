using Microsoft.AspNetCore.Builder;

namespace DiaryScheduler.Api.Exceptions;

public static class ExceptionConfiguration
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
