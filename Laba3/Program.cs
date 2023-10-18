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

        var app = builder.Build();

        //app.MapGet("/", () => "Hello World!");

        app.Map("/info", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
 
                string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>����������:</H1>";
                strResponse += "<BR> ������: " + context.Request.Host;
                strResponse += "<BR> ����: " + context.Request.PathBase;
                strResponse += "<BR> ��������: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";

                await context.Response.WriteAsync(strResponse);
            });
        });

        app.Map("/employee", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<Employee> employees = cachedSubsCityDb.GetEmployee(CacheKey.Employee);

                string HtmlString = "<HTML><HEAD><TITLE>���������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>���</TH>";
                HtmlString += "<TH>�������</TH>";
                HtmlString += "<TH>��������</TH>";;
                HtmlString += "</TR>";
                foreach (var employee in employees)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + employee.Id + "</TD>";
                    HtmlString += "<TD>" + employee.Name + "</TD>";
                    HtmlString += "<TD>" + employee.Middlename + "</TD>";
                    HtmlString += "<TD>" + employee.Surname + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });

        app.Map("/employee_posutions", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<EmployeePosition> employeePositions = cachedSubsCityDb.GetEmployeePosition(CacheKey.EmployeePosition);

                string HtmlString = "<HTML><HEAD><TITLE>���������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>���������</TH>";; 
                HtmlString += "</TR>";
                foreach (var position in employeePositions)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + position.Id + "</TD>";
                    HtmlString += "<TD>" + position.Position + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });

        app.Map("/office", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<Office> offices = cachedSubsCityDb.GetOffice(CacheKey.Office);

                string HtmlString = "<HTML><HEAD><TITLE>���������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>��������� �������</TH>";
                HtmlString += "<TH>email</TH>";
                HtmlString += "</TR>";
                foreach (var office in offices)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + office.Id + "</TD>";
                    HtmlString += "<TD>" + $"{office.OnwnerSurname} {office.OwnerName} {office.OwnerMiddlename}" + "</TD>";
                    HtmlString += "<TD>" + $"{office.StreetName}" + "</TD>";
                    HtmlString += "<TD>" + office.MobilePhone + "</TD>";
                    HtmlString += "<TD>" + office.Email + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });

        app.Map("/publication", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<Publication> publications = cachedSubsCityDb.GetPublication(CacheKey.Publication);

                string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ �������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "<TH>����</TH>";
                HtmlString += "</TR>";
                foreach (var publication in publications)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + publication.Id + "</TD>";
                    HtmlString += "<TD>" + publication.Name + "</TD>";
                    HtmlString += "<TD>" + publication.Price + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });

        app.Map("/publication_type", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<PublicationType> publicationsTypes = cachedSubsCityDb.GetPublicationType(CacheKey.PyblicationType);

                string HtmlString = "<HTML><HEAD><TITLE>���� �������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ����� �������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>���</TH>";
                HtmlString += "</TR>";
                foreach (var publicationType in publicationsTypes)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + publicationType.Id + "</TD>";
                    HtmlString += "<TD>" + publicationType.Type + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });

        app.Map("/recipient", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<Recipient> recipients = cachedSubsCityDb.GetRecipient(CacheKey.Recipient);

                string HtmlString = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ �����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>����������</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>��������� �������</TH>";
                HtmlString += "<TH>email</TH>";
                HtmlString += "</TR>";
                foreach (var recipient in recipients)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + recipient.Id + "</TD>";
                    HtmlString += "<TD>" + $"{recipient.Surname} {recipient.Name} {recipient.Middlename}" + "</TD>";
                    HtmlString += "<TD>" + $"{recipient.AddressId}" + "</TD>";
                    HtmlString += "<TD>" + recipient.MobilePhone + "</TD>";
                    HtmlString += "<TD>" + recipient.Email + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });

        app.Map("/recipient_address", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<RecipientAddress> recipientAddresses = cachedSubsCityDb.GetRecipientAddress(CacheKey.RecipientAddress);

                string HtmlString = "<HTML><HEAD><TITLE>������ �����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ������� �����������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>���</TH>";
                HtmlString += "<TH>��������</TH>";
                HtmlString += "</TR>";
                foreach (var recipientAddress in recipientAddresses)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + recipientAddress.Id + "</TD>";
                    HtmlString += "<TD>" + recipientAddress.Street + "</TD>";
                    HtmlString += "<TD>" + recipientAddress.House + "</TD>";
                    HtmlString += "<TD>" + recipientAddress.Apartment + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });

        app.Map("/subscription", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

                IEnumerable<Subscription> subscriptions = cachedSubsCityDb.GetSubscription(CacheKey.Subscription);

                string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ��������</H1>" +
                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>�����������������</TH>";
                HtmlString += "<TH>���� ��������</TH>";
                HtmlString += "<TH>����������</TH>";
                HtmlString += "<TH>�������</TH>";
                HtmlString += "<TH>����</TH>";
                HtmlString += "</TR>";
                foreach (var subscription in subscriptions)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + subscription.Id + "</TD>";
                    HtmlString += "<TD>" + subscription.Duration + "</TD>";
                    HtmlString += "<TD>" + subscription.SubscriptionStartDate + "</TD>";
                    HtmlString += "<TD>" + subscription.Recipient + "</TD>";
                    HtmlString += "<TD>" + subscription.Publication + "</TD>";
                    HtmlString += "<TD>" + subscription.Office + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);

            });
        });



        app.Run((context) =>
        {
            
            CachedSubsCityDb cachedSubsCityDb = context.RequestServices.GetService<CachedSubsCityDb>();

            cachedSubsCityDb.AddEmployeeToCache(CacheKey.Employee);
            cachedSubsCityDb.AddEmployeePositionToCache(CacheKey.EmployeePosition);
            cachedSubsCityDb.AddOfficeToCache(CacheKey.Office);
            cachedSubsCityDb.AddPuplicationToCache(CacheKey.Publication);
            cachedSubsCityDb.AddPuplicationTypeToCache(CacheKey.PyblicationType);
            cachedSubsCityDb.AddRecipientToCache(CacheKey.Recipient);
            cachedSubsCityDb.AddRecipientAddressToCache(CacheKey.RecipientAddress); 
            cachedSubsCityDb.AddSubscriptionToCache(CacheKey.Subscription);

            string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>�������</H1>";
            HtmlString += "<H2>������ �������� � ��� �������</H2>";
            HtmlString += "<BR><A href='/'>�������</A></BR>";
            HtmlString += "<BR><A href='/employee'>���������</A></BR>";
            HtmlString += "<BR><A href='/employee_posutions'>���������</A></BR>";
            HtmlString += "<BR><A href='/office'>�����</A></BR>";
            HtmlString += "<BR><A href='/publication'>�������</A></BR>";
            HtmlString += "<BR><A href='/publication_type'>���� �������</A></BR>";
            HtmlString += "<BR><A href='/recipient'>����������</A></BR>"; 
            HtmlString += "<BR><A href='/recipient_address'>������ �����������</A></BR>";
            HtmlString += "<BR><A href='/subscription'>��������</A></BR>";
            HtmlString += "</BODY></HTML>";

            return context.Response.WriteAsync(HtmlString);

        });

        app.Run();
    }
}