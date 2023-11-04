using Laba4.Middleware;
using Laba4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

string? connection = builder.Configuration.GetConnectionString("SqlServerConnection");
services.AddDbContext<SubsCityContext>(options => options.UseSqlServer(connection));


builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews(options => {
    options.CacheProfiles.Add("DbTableCache",
        new CacheProfile()
        {
            Location = ResponseCacheLocation.Any,
            Duration = 2 * 2 + 240
        });
});

var app = builder.Build();

app.UseSession();
app.UseDbInitializerMiddleware();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Employee Position",
    pattern: "{controller=EmployeePosition}/{action=ShowEmployeePosition}");

app.MapControllerRoute(
    name: "Employee",
    pattern: "{controller=Employee}/{action=ShowEmployee}");

app.MapControllerRoute(
    name: "Office",
    pattern: "{controller=Office}/{action=ShowOffice}");

app.Run();
