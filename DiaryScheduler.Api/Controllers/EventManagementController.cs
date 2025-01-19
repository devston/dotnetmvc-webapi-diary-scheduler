using Asp.Versioning;
using DiaryScheduler.Api.Services.EventManagement;
using DiaryScheduler.Presentation.Models.Base;
using DiaryScheduler.Presentation.Models.Scheduler;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Event Management")]
public class EventManagementController : Controller
{
    private readonly IEventManagementApiService _eventManagementApiService;
    private readonly IValidator<CalendarEventViewModel> _eventValidator;
    private readonly ILogger _logger;

    public EventManagementController(
        IEventManagementApiService eventManagementApiService,
        IValidator<CalendarEventViewModel> eventValidator,
        ILogger<EventManagementController> logger)
    {
        _eventManagementApiService = eventManagementApiService;
        _eventValidator = eventValidator;
        _logger = logger;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEventsBetweenDateRangeAsync(DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting events between date range {start} - {end}.", start, end);
        return Ok(await _eventManagementApiService.GetCalendarEventsBetweenDateRangeAsync(start, end, cancellationToken));
    }

    [HttpGet("events/{id}")]
    public async Task<IActionResult> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting the event with id = {id}", id);
        return Ok(await _eventManagementApiService.GetCalendarEventByIdAsync(id, cancellationToken));
    }

    [HttpPost("events")]
    public async Task<IActionResult> CreateEventAsync([FromBody] CalendarEventViewModel calendarEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Validating the create event request with payload = {calendarEvent}", calendarEvent);
        var validationResult = await _eventValidator.ValidateAsync(calendarEvent, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors?.Select(x => new ValidationResultViewModel()
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        _logger.LogInformation("Creating the event with payload = {calendarEvent}", calendarEvent);
        return Ok(await _eventManagementApiService.CreateCalendarEventAsync(calendarEvent, cancellationToken));
    }

    [HttpPut("events/{id}")]
    public async Task<IActionResult> UpdateEventAsync(Guid id, [FromBody] CalendarEventViewModel calendarEvent, CancellationToken cancellationToken)
    {
        calendarEvent.CalendarEventId = id;
        _logger.LogInformation("Validating the update event request with payload = {calendarEvent}", calendarEvent);
        var validationResult = await _eventValidator.ValidateAsync(calendarEvent, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors?.Select(x => new ValidationResultViewModel()
            {
                PropertyName = x.PropertyName,
                Message = x.ErrorMessage
            }));
        }

        _logger.LogInformation("Updating the event with payload = {calendarEvent}", calendarEvent);
        return Ok(await _eventManagementApiService.UpdateCalendarEventAsync(calendarEvent, cancellationToken));
    }

    [HttpDelete("events/{id}")]
    public async Task<IActionResult> DeleteEventAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting the event with id = {id}", id);
        return Ok(await _eventManagementApiService.DeleteCalendarEventAsync(id, cancellationToken));
    }
}
