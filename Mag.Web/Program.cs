using System.Security.Claims;
using Mag.BL;
using Mag.BL.Extensions;
using Mag.Common;
using Mag.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mag.DAL;
using Mag.DAL.Entities;
using static Mag.BL.Extensions.DbInitExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization((opt) =>
{
    opt.AddPolicy("Root", p =>
        p.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, DefaultRoles.rootConst)));
    
    opt.AddPolicy("Admin", p =>
        p.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, DefaultRoles.adminConst)
                                || x.User.HasClaim(ClaimTypes.Role, DefaultRoles.rootConst)));
    opt.AddPolicy("User", p =>
        p.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, DefaultRoles.unknownConst)
                                || x.User.HasClaim(ClaimTypes.Role, DefaultRoles.userConst)));
});

builder.Services.AddIdentityDependency();
builder.Services.AddDependencyServices();
builder.Services.AddDependencyDbContext(builder.Configuration);

builder.Services.ConfigureApplicationCookie(conf =>
{
    conf.LoginPath = "/Account/Login";
    conf.AccessDeniedPath = "/Shared/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCookiePolicy();

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


using(var scope = app.Services.CreateScope())
{
    DbInit(scope, builder.Configuration).GetAwaiter().GetResult();
}


app.Run();