using Microsoft.EntityFrameworkCore;
using PostSity.Data;

var builder = WebApplication.CreateBuilder(args);

// Enable Services
IServiceCollection services = builder.Services;

// Add Db context
string? connection = builder.Configuration.GetConnectionString("SqlServerConnection");

services.AddDbContext<PostCityContext>(options =>
{
    options.UseSqlServer(connection);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



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
