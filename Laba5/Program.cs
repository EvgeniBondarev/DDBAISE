using Laba4.Middleware;
using Laba4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Laba4.Data;
using PostCity.Data.Cache;
using PostCity.Data.Cookies;
using PostCity.Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

string connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

builder.Services.AddDbContext<SubsCityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddDefaultIdentity<IdentityUser>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SubsCityContext>();

services.AddMemoryCache();
services.AddDistributedMemoryCache();
services.AddSession();

services.AddTransient(typeof(FilterBy<>));
services.AddTransient<CookiesManeger>();
services.AddTransient(typeof(DatabaseSaveFilter));

services.AddTransient<SubscriptionCache>();
services.AddTransient<EmployeeCache>();
services.AddTransient<OfficeCache>();

services.AddTransient<UserInitializer>();

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
