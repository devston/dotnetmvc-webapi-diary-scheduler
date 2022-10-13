using Asp.Versioning;
using Autofac;
using DiaryScheduler.Api.Configuration;
using DiaryScheduler.Api.Exceptions;
using DiaryScheduler.Api.Swagger;
using DiaryScheduler.DependencyResolution;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiaryScheduler.Api;

public class Startup
{
    private readonly string _apiVersion;
    private readonly int _apiVersionMajor;
    private readonly int _apiVersionMinor;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        _apiVersionMajor = Configuration.GetValue<int>("ApiVersionMajor");
        _apiVersionMinor = Configuration.GetValue<int>("ApiVersionMinor");
        _apiVersion = $"{_apiVersionMajor}.{_apiVersionMinor}";
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureDbContextService(Configuration);
        services.AddControllers(options => 
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });
        services.ConfigureSwaggerServices();
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(_apiVersionMajor, _apiVersionMinor);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.ConfigureCustomExceptionMiddleware();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.ConfigureSwagger();
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        IoCBootstrapper.ConfigureContainer(builder);
    }
}
