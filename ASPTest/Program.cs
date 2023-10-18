namespace ASPTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.MapGet("/test", () => $"<H1>Hello World!</H1>");

            app.Run();
        }
    }
}