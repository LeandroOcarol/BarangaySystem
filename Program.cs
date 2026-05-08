using Microsoft.EntityFrameworkCore;
using BarangaySystem.Data;
using BarangaySystem.Models;
using BarangaySystem.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC
builder.Services.AddControllersWithViews();

// 2. Add the database connection
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Add login/logout system (cookie-based)
builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
{
   options.LoginPath = "/Account/Login"; // Redirect here if not logged in
   options.AccessDeniedPath = "/Account/AccessDenied";
   options.ExpireTimeSpan = TimeSpan.FromHours(8); 
});

builder.Services.AddAuthorization();

// 4. Register PdfService so it can be injected into controllers
builder.Services.AddScoped<PdfService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // MUST be before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();