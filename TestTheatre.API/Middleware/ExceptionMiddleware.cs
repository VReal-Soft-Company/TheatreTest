using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTheare.Shared.Data.Exceptions;

namespace TestTheatre.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                await next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning($"{DateTime.Now} :get app exception with next message :{ex.Message}");
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                _logger.LogWarning($"{DateTime.Now} :get app exception with next message :{e.Message}");
                await context.Response.WriteAsync((e.InnerException != null ? e.InnerException.StackTrace + e.InnerException.Message : "") + e.Message + e.StackTrace);
            }
        }

    }
}
