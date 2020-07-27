using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Transaction.WebApi.Middleware
{
    public class TransactionErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public TransactionErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);                
            }
            catch (Exception ex)
            {
                await HandleExceptionsAsync(context, ex);
            }
        }
        public static Task HandleExceptionsAsync(HttpContext context, Exception ex)
        {
            //write to log db the exception
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new { error = ex });
            return context.Response.WriteAsync(result);
        }
    }
}
