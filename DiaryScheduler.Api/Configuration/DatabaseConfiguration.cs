using DiaryScheduler.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiaryScheduler.Api.Configuration;

public static class DatabaseConfiguration
{
    public static void ConfigureDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options => {
            options.EnableSensitiveDataLogging(true);
            options.UseSqlServer(connectionString,
                sqlOptionsBuilder =>
                {
                    sqlOptionsBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null);
                });
        }, ServiceLifetime.Transient);
    }
}
