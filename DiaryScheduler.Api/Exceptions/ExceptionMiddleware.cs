using DiaryScheduler.Presentation.Models.Base;
using DiaryScheduler.ScheduleManagement.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string errorMessage = "Something went wrong";
        int statusCode = (int)HttpStatusCode.InternalServerError;

        if (exception.GetType() == typeof(ScheduleManagementEventNotFoundException))
        {
            errorMessage = exception.Message;
            statusCode = (int)HttpStatusCode.BadRequest;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var errorJson = JsonSerializer.Serialize(new BaseResponseViewModel()
        {
            StatusCode = context.Response.StatusCode,
            Message = errorMessage
        });

        await context.Response.WriteAsync(errorJson);
    }
}
