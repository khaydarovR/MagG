using Mag.BL;
using Mag.BL.Extensions;
using Mag.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mag.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

builder.Services.AddIdentityDependency();
builder.Services.AddDependencyServices();
builder.Services.AddDependencyDbContext(builder.Configuration);

builder.Services.ConfigureApplicationCookie(conf =>
{
    conf.LoginPath = "/Account/Login";
    conf.AccessDeniedPath = "/Shared/Error";
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
await scope.DbInit(builder.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name : "def",
        pattern : "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();