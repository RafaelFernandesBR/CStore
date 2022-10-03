using LojaAc.Data.IModels;
using LojaAc.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options => options.LoginPath = "/Home/DefBoard");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDataModel, DataModel>();
builder.Services.AddScoped<IAutenticaUserModel, AutenticaUserModel>();
builder.Services.AddScoped<ILogsData, LogsData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
