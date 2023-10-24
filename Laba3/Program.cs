using Laba3;
using Laba3.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;

        string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
        services.AddDbContext<SubsCityContext>(options => options.UseSqlServer(connection));

        

        services.AddMemoryCache();

        services.AddDistributedMemoryCache();
        services.AddScoped<CachedSubsCityDb>();
        services.AddSession();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(); 

        var app = builder.Build();
        app.UseSession();


        app.Map("/info", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
 
                string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Информация:</H1>";
                strResponse += "<BR> Сервер: " + context.Request.Host;
                strResponse += "<BR> Путь: " + context.Request.PathBase;
                strResponse += "<BR> Протокол: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";

                await context.Response.WriteAsync(strResponse);
            });
        });

        app.Map("/subscription", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();
                TableWriter tableWriter = new TableWriter();

                IEnumerable<Subscription> subscriptions = cachedSubsCityDb.GetSubscription(CacheKey.Subscription);
                string HtmlString = tableWriter.WriteTable(subscriptions);

                Console.WriteLine(HtmlString == null);

                await context.Response.WriteAsync(HtmlString);
            });
        });

        app.Map("/searchPublicationName", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();
                IEnumerable<Subscription> subscriptions = cachedSubsCityDb.GetSubscription(CacheKey.Subscription);

                TableWriter tableWriter = new TableWriter();

                string formHtml = "<form method='post' action='/searchPublicationName'>" +
                                  "<label for='name'>Название издания:</label>";



                if (context.Request.Cookies.TryGetValue("name", out var input_value))
                {
                    formHtml += $"<input type='text' name='name' value='{input_value}'><br><br>" +
                               "<input type='submit' value='Поиск'>" +
                               "</form>"; 
                }
                else
                {
                    formHtml += "<input type='text' name='name'><br><br>" + 
                                "<input type='submit' value='Поиск'>" +
                                "</form>";
                }


                if (context.Request.Method == "POST")
                {
                    string name = context.Request.Form["name"];

                    context.Response.Cookies.Append("name", name);

                    IEnumerable<Subscription> subscriptionsByPublications = subscriptions.Where(s => s.Publication.Name == name);

                    string HtmlString = tableWriter.WriteTable(subscriptionsByPublications, formHtml);

                   await context.Response.WriteAsync(HtmlString);
                }
                else
                {
                    string HtmlString = tableWriter.WriteTable(subscriptions, formHtml);
                   
                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });
        app.Map("/searchDuration", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();
                IEnumerable<Subscription> subscriptions = cachedSubsCityDb.GetSubscription(CacheKey.Subscription);

                TableWriter tableWriter = new TableWriter();

                string formHtml = "<form method='post' action='/searchDuration'>" +
                                    "<label for='name'>Минимальная продолжителность:</label>";
                                    

                if (context.Session.Keys.Contains("duration"))
                {
                    int duration = Int32.Parse(context.Session.GetString("duration"));

                    formHtml += $"<input type='number' name='duration' value='{duration}'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                 "</form>";
                }
                else
                {
                    formHtml += "<input type='number' name='duration'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                 "</form>";
                }

                if (context.Request.Method == "POST")
                {
                    int duration = Int32.Parse(context.Request.Form["duration"]);

                    context.Session.SetString("duration", duration.ToString());

                    IEnumerable<Subscription> subscriptionsByPublications = subscriptions.Where(s => s.Duration >= duration);

                    string HtmlString = tableWriter.WriteTable(subscriptionsByPublications, formHtml);


                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {

                    string HtmlString = tableWriter.WriteTable(subscriptions, formHtml);


                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });





        app.Run((context) =>
        {
            
            CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

            cachedSubsCityDb.AddOfficeToCache(CacheKey.Office);
            cachedSubsCityDb.AddPuplicationToCache(CacheKey.Publication);
            cachedSubsCityDb.AddRecipientToCache(CacheKey.Recipient);
            cachedSubsCityDb.AddSubscriptionToCache(CacheKey.Subscription);

            string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>Главная</H1>";
            HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
            HtmlString += "<BR><A href='/'>Главная</A></BR>";
            HtmlString += "<BR><A href='/info'>Информация</A></BR>";
            HtmlString += "<BR><A href='/subscription'>Все подписки</A></BR>";
            HtmlString += "<BR><A href='/searchPublicationName'>Поиск по изданию</A></BR>";
            HtmlString += "<BR><A href='/searchDuration'>Поиск по продолжиьельности</A></BR>";
            HtmlString += "</BODY></HTML>";

            return context.Response.WriteAsync(HtmlString);

        });

        app.Run();
    }
}