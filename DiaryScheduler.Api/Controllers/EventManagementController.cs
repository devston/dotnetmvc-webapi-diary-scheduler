using DiaryScheduler.Api.Services.EventManagement;
using DiaryScheduler.Presentation.Models.Scheduler;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Event Management")]
    public class EventManagementController : Controller
    {
        private readonly IEventManagementApiService _eventManagementApiService;

        public EventManagementController(
            IEventManagementApiService eventManagementApiService)
        {
            _eventManagementApiService = eventManagementApiService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEventsBetweenDateRangeAsync(DateTime start, DateTime end)
        {
            try
            {
                return Ok(await _eventManagementApiService.GetCalendarEventsBetweenDateRangeAsync(start, end));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("events/{id}")]
        public async Task<IActionResult> GetEventByIdAsync(Guid id)
        {
            try
            {
                return Ok(await _eventManagementApiService.GetCalendarEventByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("events")]
        public async Task<IActionResult> CreateEventAsync([FromBody] CalendarEventViewModel calendarEvent)
        {
            try
            {
                return Ok(await _eventManagementApiService.CreateCalendarEventAsync(calendarEvent));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("events/{id}")]
        public async Task<IActionResult> UpdateEventAsync(Guid id, [FromBody] CalendarEventViewModel calendarEvent)
        {
            try
            {
                calendarEvent.CalendarEventId = id;
                return Ok(await _eventManagementApiService.UpdateCalendarEventAsync(calendarEvent));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("events/{id}")]
        public async Task<IActionResult> DeleteEventAsync(Guid id)
        {
            try
            {
                return Ok(await _eventManagementApiService.DeleteCalendarEventAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
