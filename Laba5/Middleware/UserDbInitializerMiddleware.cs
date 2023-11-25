using Laba4.Data;
using Laba4.Models;

namespace Laba4.Middleware
{
    public class UserDbInitializerMiddleware
    {
            private readonly RequestDelegate _next;

            public UserDbInitializerMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public Task Invoke(HttpContext httpContext, UserInitializer Initializer)
            {
                if (!(httpContext.Session.Keys.Contains("startingUser")))
                {
                    UserInitializer userInitializer = Initializer;

                    userInitializer.Initialize();

                    httpContext.Session.SetString("startingUser", "Yes");
                }
                return _next.Invoke(httpContext);
            
            }
    }
    public static class UserDbInitializerMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserDbInitializerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserDbInitializerMiddleware>();
        }
    }

}
