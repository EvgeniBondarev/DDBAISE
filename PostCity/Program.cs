
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PostCity.Infrastructure.Filters;
using Domains.Models;
using PostCity.Middleware;
using PostCity;
using Repository.Data;
using Repository.Models;
using Service.Data.Cache;
using Service.Data.Cookies;
using Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

IServiceCollection services = builder.Services;

string connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

builder.Services.AddDbContext<PostCityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddDefaultIdentity<PostCityUser>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PostCityContext>();

services.AddTransient<UserRegistrationManager>();




services.AddMemoryCache();
services.AddDistributedMemoryCache();
services.AddSession();

services.AddTransient(typeof(FilterBy<>));
services.AddTransient<CookiesManeger>();
services.AddTransient(typeof(DatabaseSaveFilter));


services.AddTransient<SubscriptionCache>();
services.AddTransient<EmployeeCache>();
services.AddTransient<OfficeCache>();
services.AddTransient<RecipientCache>();
services.AddTransient<UserCache>();
services.AddTransient<PublicationCache>();
services.AddTransient<CacheUpdater>();

services.AddTransient<DbInitializer>();

services.AddTransient<SessionLogger>();





var app = builder.Build();

app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseDbInitializerMiddleware();
app.UseStaticFiles();
app.UseRouting();

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();