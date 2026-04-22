using GLMS.Data;
using GLMS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add services
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddHttpClient<CurrencyService>();

var app = builder.Build();

// 🔹 Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ✅ REQUIRED for wwwroot (uploads, css, js)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 🔹 Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();