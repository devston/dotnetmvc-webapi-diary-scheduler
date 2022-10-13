using Autofac;
using Microsoft.Extensions.Logging;

namespace DiaryScheduler.DependencyResolution.Modules;

/// <summary>
/// The logging autofac module.
/// </summary>
public class LoggingModule : Module
{
    /// <summary>
    /// Load the module.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();

        builder.RegisterGeneric(typeof(Logger<>))
               .As(typeof(ILogger<>))
               .SingleInstance();
    }
}
