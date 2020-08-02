using Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Account.WebApi.Miidleware
{
    public class AccountErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public AccountErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    throw new UnauthorizedAccessException("Password or Email are not correct. try again!");
                }
                if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    throw new UnauthorizedAccessException("Action Failed. Please try again!");
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionsAsync(context, ex);
            }
        }
        public static Task HandleExceptionsAsync(HttpContext context, Exception ex)
        {
            var code = new HttpStatusCode();
            var errorMessage = "";
            if (ex is AccountNotFoundException)
            {
                code = HttpStatusCode.Unauthorized;
                context.Response.StatusCode = (int)code;
                errorMessage = "Email or password is not correct.";
            }
            if (ex is DuplicateEmailException)
            {
                code = HttpStatusCode.BadRequest;
                context.Response.StatusCode = (int)code;
                errorMessage = "One of your details is not valid. Try again.";
            }
            if (ex is VerificationCodeNotValidException)
            {
                code = HttpStatusCode.BadRequest;
                context.Response.StatusCode = (int)code;
                errorMessage = "Your Verification Code is not valid or expire time.Please Try again.";
            }
            var result = JsonConvert.SerializeObject(new { error = errorMessage });
            return context.Response.WriteAsync(result);
        }
    }
}
