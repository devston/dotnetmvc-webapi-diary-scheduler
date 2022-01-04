using Autofac;
using DiaryScheduler.Presentation.Services.Scheduler;
using DiaryScheduler.Presentation.Services.Utility;

namespace DiaryScheduler.DependencyResolution.Modules
{
    /// <summary>
    /// The presentation services autofac module.
    /// </summary>
    public class PresentationModule : Module
    {
        /// <summary>
        /// Load the module.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DateTimeService>()
                .As<IDateTimeService>()
                .SingleInstance();

            builder.RegisterType<SchedulerPresentationService>()
                .As<ISchedulerPresentationService>()
                .InstancePerLifetimeScope();
        }
    }
}
