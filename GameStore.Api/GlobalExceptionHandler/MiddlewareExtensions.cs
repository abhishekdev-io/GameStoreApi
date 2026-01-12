namespace GameStore.Api.GlobalExceptionHandler
{
    public static class MiddlewareExtensions
    {

        //Create an extension method in IApplicationBuilder which register ExceptionMiddleware automatically while calling
        //UseGlobalExceptionHandler() in program.cs.
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
