using DiaryScheduler.Presentation.Models.Scheduler;
using FluentValidation;
using System;

namespace DiaryScheduler.Api.Services.Validators.Events;

public class CalendarEventViewModelValidator : AbstractValidator<CalendarEventViewModel>
{
	public CalendarEventViewModelValidator()
	{
		RuleFor(x => x.DateFrom).GreaterThan(DateTime.MinValue);
		RuleFor(x => x.DateTo).GreaterThan(x => x.DateFrom);
		RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
		RuleFor(x => x.Description).MaximumLength(200);
	}
}
