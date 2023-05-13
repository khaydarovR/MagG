using Microsoft.EntityFrameworkCore;
using Mag.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Mag.BL.Extensions;

public static class AddDbContext
{
    public static IServiceCollection AddDependencyDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>((options) =>
        {
            options.UseMySql(
                    serverVersion: new MySqlServerVersion("8.0.32"),
                    connectionString: config.GetConnectionString("MySql")
                );
        });

        return services;
    }
}