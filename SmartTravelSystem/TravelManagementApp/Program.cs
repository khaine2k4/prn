using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1) Register DbContext (đọc từ appsettings.json)
builder.Services.AddDbContext<TravelCenterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBTravelCenter")));

// 2) Enable Session (để giữ CustomerID sau login)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();      // << quan trọng: phải đặt trước Authorization

app.UseAuthorization();

// default route: vào Login trước cho đúng đề
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();