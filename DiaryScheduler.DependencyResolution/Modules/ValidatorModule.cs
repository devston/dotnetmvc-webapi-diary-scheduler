using Autofac;
using DiaryScheduler.Api.Services.Validators.Events;
using DiaryScheduler.Presentation.Models.Scheduler;
using FluentValidation;

namespace DiaryScheduler.DependencyResolution.Modules;

/// <summary>
/// The validator autofac module.
/// </summary>
public class ValidatorModule : Module
{
    /// <summary>
    /// Load the module.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CalendarEventViewModelValidator>()
            .As<IValidator<CalendarEventViewModel>>()
            .InstancePerLifetimeScope();
    }
}
