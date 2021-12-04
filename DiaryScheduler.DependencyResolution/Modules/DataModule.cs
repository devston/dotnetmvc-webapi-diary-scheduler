using Autofac;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Data.Repositories;
using DiaryScheduler.ScheduleManagement.Data.Services;

namespace DiaryScheduler.DependencyResolution.Modules
{
    /// <summary>
    /// The data autofac module.
    /// </summary>
    public class DataModule : Module
    {
        /// <summary>
        /// Load the module.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainMapperService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EFScheduleRepository>()
                .As<IScheduleRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
