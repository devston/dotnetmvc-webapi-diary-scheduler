using Autofac;
using DiaryScheduler.DependencyResolution.Modules;

namespace DiaryScheduler.DependencyResolution;

/// <summary>
/// The IoC container bootstrapper.
/// </summary>
public class IoCBootstrapper
{
    /// <summary>
    /// Gets or sets the IoC container.
    /// </summary>
    private static IContainer Container { get; set; }

    /// <summary>
    /// Initialise the container.
    /// </summary>
    /// <returns>The IoC container.</returns>
    public static IContainer Initialise()
    {
        // Build the container.
        BuildContainer();

        // Return the built container.
        return Container;
    }

    /// <summary>
    /// Set up the builder. This is exposed so we can continue building the container in the UI layer where needed.
    /// </summary>
    /// <returns>The container builder.</returns>
    public static ContainerBuilder SetUpBuilder()
    {
        // Create the container builder.
        var builder = new ContainerBuilder();

        // Do any registrations.
        builder.RegisterModule<LoggingModule>();
        builder.RegisterModule<DataModule>();
        builder.RegisterModule<ApiServiceModule>();

        // Return the builder.
        return builder;
    }

    /// <summary>
    /// Configure IoC Container using Autofac: Register DI.
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule<LoggingModule>();
        builder.RegisterModule<DataModule>();
        builder.RegisterModule<ApiServiceModule>();
        builder.RegisterModule<ValidatorModule>();
    }

    /// <summary>
    /// Get the built IoC container.
    /// </summary>
    /// <returns>The IoC container.</returns>
    public static IContainer GetIoCContainer()
    {
        // Check if the container has been built.
        if (Container == null)
        {
            BuildContainer();
        }

        return Container;
    }

    /// <summary>
    /// Set the IoC container.
    /// </summary>
    /// <param name="container">The provided container.</param>
    public static void SetIoCContainer(IContainer container)
    {
        Container = container;
    }

    /// <summary>
    /// Dispose of the IoC container.
    /// </summary>
    public static void DisposeContainer()
    {
        if (Container != null)
        {
            Container.Dispose();
        }
    }

    /// <summary>
    /// Build the container.
    /// </summary>
    private static void BuildContainer()
    {
        // Create the container builder.
        var builder = SetUpBuilder();

        // Build the container.
        Container = builder.Build();
    }
}
