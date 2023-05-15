using Mag.Common.Interfaces;
using Mag.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Mag.BL.Extensions;

public static class AddServices
{
    public static IServiceCollection AddDependencyServices(this IServiceCollection services) 
    {
        services.AddScoped<IAccountService<AppUser>, AccountService>();
        services.AddScoped<UserService>();
        
        
        //services.AddScoped<EmailService>();
        return services;
    }
}