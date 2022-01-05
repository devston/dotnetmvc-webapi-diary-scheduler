using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace DiaryScheduler.Api.Swagger
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scheduler API v1");
            });
        }

        public static void ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var version = "v1";
                c.SwaggerDoc(version, new OpenApiInfo { Title = "Scheduler API", Version = version });

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });

                c.DocInclusionPredicate((name, api) => true);

                c.AddEnumsWithValuesFixFilters(services, o =>
                {
                    o.IncludeDescriptions = true;
                    o.ApplyDocumentFilter = true;
                });
            });
        }
    }
}
