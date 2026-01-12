using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace GameStore.Api.GlobalExceptionHandler
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IWebHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger, IWebHostEnvironment _env)
        {
            next = _next;
            logger = _logger;
            env = _env;
        }

        //.NET Automatically Calls InvokeAsync() For Every Requests and send the HttpContext of the request.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception exe)
            {
                //Generate a Unique Id for the error for Identification and Correlation.

                var errorId = Guid.NewGuid(); //Generate the Id.

                //Record the error inside the Error Log.
                logger.LogError
                (
                    exe,
                    "ErrorId: {ErrorId} - {Method} {Path} - {Message}",
                    errorId,
                    context.Request.Method,
                    context.Request.Path,
                    exe.Message
                );

                await HandleExceptionAsync(context, exe, errorId);

            }
        }

        //Convert C# Exception to proper HTTP Response.
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, Guid errorId)
        {
            if (context.Response.HasStarted)
                return;

            context.Response.Clear();


            //Default to 500 Internal Server Error
            var statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occured.";

            //Customise Response based on Exception type

            switch (exception) 
            {
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;

                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                //Add More custom Exceptions Here...
            }


            //Build the Response Object.
            var problem = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = message,
                Detail = env.IsDevelopment() ? exception.ToString() : null, //StackTrace
                Instance = context.Request.Path
            };
            problem.Extensions["errorId"] = errorId;

            //Convert C# object to Json and format return data as CamelCase.
            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });


            //Telling the Browser/FrontEnd that the response is json data.
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)statusCode;

            //Write the response to HTTP response body.
            await context.Response.WriteAsync(json);
        }
    }
}
