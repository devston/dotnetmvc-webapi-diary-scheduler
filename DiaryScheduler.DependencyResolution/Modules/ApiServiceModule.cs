using Autofac;
using DiaryScheduler.Api.Services.EventManagement;

namespace DiaryScheduler.DependencyResolution.Modules;

/// <summary>
/// The api services autofac module.
/// </summary>
public class ApiServiceModule : Module
{
    /// <summary>
    /// Load the module.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EventManagementApiService>()
            .As<IEventManagementApiService>()
            .InstancePerLifetimeScope();
    }
}
