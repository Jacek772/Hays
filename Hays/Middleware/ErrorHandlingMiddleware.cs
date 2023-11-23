namespace Hays.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception ex)
            {
                //context.Response.StatusCode = 500;
                //await context.Response.WriteAsJsonAsync(new ErrorDTO("SerwerError", "Something went wrong")
                //{
                //    Title = "Serwer error",
                //    Status = 500
                //});
            }
        }
    }
}
