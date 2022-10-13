using DiaryScheduler.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace DiaryScheduler.Api.Integration.Tests.Configuration;

public static class Utilities
{
    public static void ConfigureServices(IServiceCollection services)
    {
        string databaseName = Guid.NewGuid().ToString();

        services.RemoveAll(typeof(DbContext));
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName);
        }, ServiceLifetime.Transient);

        var sp = services.BuildServiceProvider();

        using (var scope = sp.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            try
            {
                // Seed the database.
                InitialiseDbForTests(db);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    private static void InitialiseDbForTests(ApplicationDbContext appContext)
    {
        appContext.Database.EnsureDeleted();
        appContext.Database.EnsureCreated();
        appContext.CalendarEvents.Add(DbTestModels.Event1);
        appContext.SaveChanges();
    }
}
