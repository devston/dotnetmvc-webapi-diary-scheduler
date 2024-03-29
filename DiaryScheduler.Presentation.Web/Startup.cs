using Autofac;
using DiaryScheduler.Presentation.Services.Configuration;
using DiaryScheduler.Presentation.Services.Scheduler;
using DiaryScheduler.Presentation.Services.Utility;
using DiaryScheduler.Presentation.Web.Common.Services.Scheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using System;

namespace DiaryScheduler.Presentation.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var apiConfig = Configuration.GetSection(EventApiConfig.SectionName).Get<EventApiConfig>();
        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddRefitClient<IEventApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfig.Url));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<SchedulerUrlGenerationService>()
            .As<ISchedulerUrlGenerationService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DateTimeService>()
            .As<IDateTimeService>()
            .SingleInstance();

        builder.RegisterType<SchedulerPresentationService>()
            .As<ISchedulerPresentationService>()
            .InstancePerLifetimeScope();
    }
}
