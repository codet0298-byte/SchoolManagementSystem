using Microsoft.EntityFrameworkCore;
using System;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://0.0.0.0:7108", "http://0.0.0.0:5108");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllersWithViews();
// إعدادات المصادقة هنا
// builder.Build)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // إذا لم يكن مسجلاً، حوله لهذه الصفحة تلقائياً
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
// 2. تفعيل المصادقة والتفويض هنا (بالترتيب الصحيح بعد UseRouting وقبل MapControllerRoute)
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
