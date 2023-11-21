using Microsoft.EntityFrameworkCore;
using PostCity.Data;
using PostCity.Data.Cache;
using PostCity.Data.Cookies;
using PostCity.Infrastructure.Filters;
using PostCity.Middleware;
using PostCity.ViewModels.Filters;

var builder = WebApplication.CreateBuilder(args);

// Enable Services
IServiceCollection services = builder.Services;

// Add Db context
string? connection = builder.Configuration.GetConnectionString("SqlServerConnection");

services.AddDbContext<PostCityContext>(options =>
{
    options.UseSqlServer(connection);
});


services.AddMemoryCache();
services.AddDistributedMemoryCache();
services.AddSession();

services.AddTransient(typeof(FilterBy<>));
services.AddTransient<CookiesManeger>();
services.AddTransient(typeof(DatabaseSaveFilter));

services.AddTransient<SubscriptionCache>();
services.AddTransient<EmployeeCache>();
services.AddTransient<OfficeCache>();




// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();
app.UseDbInitializerMiddleware();

// Configure the HTTP request pipeline.
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

app.Run();
