using MoviesApi.Exceptions;
using MoviesApp.Exceptions;

namespace MoviesApi.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (PageOutOfRangeException pageOutOfRangeException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(pageOutOfRangeException.Message);
            }
            catch (WrongPasswordException wrongPasswordException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(wrongPasswordException.Message);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
