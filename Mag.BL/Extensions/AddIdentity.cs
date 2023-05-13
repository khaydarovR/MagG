using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Mag.DAL;
using Mag.DAL.Entities;

namespace Mag.BL.Extensions;

public static class AddIdentity
{
    public static IServiceCollection AddIdentityDependency(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true && options.User.RequireUniqueEmail;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddUserManager<UserManager<AppUser>>()
            .AddDefaultTokenProviders();

        return services;
    }
}