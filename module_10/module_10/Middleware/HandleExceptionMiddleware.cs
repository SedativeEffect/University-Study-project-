using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using module_10.BLL.Exceptions;
using module_10.BLL.Exceptions.Abstract;
using module_10.Models;

namespace module_10.Middleware
{
    public class HandleExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleExceptionMiddleware(RequestDelegate next)
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
            context.Response.ContentType = "application/json; charset=utf-8";
            var (message, code) = exception switch
            {
                FormatException => (exception.Message, (int)HttpStatusCode.BadRequest),
                NotExistException => (exception.Message, NotExistException.StatusCode),
                AlreadyExistException => (exception.Message, AlreadyExistException.StatusCode),
                LectorException => (exception.Message, LectorException.StatusCode),
                AttendanceException => (exception.Message, AttendanceException.StatusCode),
                SendMailException => (exception.Message, SendMailException.StatusCode),
                _ => (exception.Message, (int)HttpStatusCode.InternalServerError)
            };
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode = code,
                Message = message
            }.ToString());
        }


    }
}
