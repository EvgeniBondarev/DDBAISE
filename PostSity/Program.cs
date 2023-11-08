using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PostSity.Data;
using PostSity.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Enable Services
IServiceCollection services = builder.Services;

// Add Db context
string? connection = builder.Configuration.GetConnectionString("SqlServerConnection");

services.AddDbContext<PostCityContext>(options =>
{
    options.UseSqlServer(connection);
});

services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

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

app.MapControllerRoute(
    name: "Employee",
    pattern: "{controller=Employee}/{action=ShowTable}");

app.Run();
