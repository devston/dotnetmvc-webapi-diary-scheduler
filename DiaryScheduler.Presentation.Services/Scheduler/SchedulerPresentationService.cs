using DiaryScheduler.Presentation.Models.Scheduler;
using DiaryScheduler.Presentation.Services.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryScheduler.Presentation.Services.Scheduler;

/// <summary>
/// The mvc implementation of the <see cref="ISchedulerPresentationService"/>.
/// </summary>
public class SchedulerPresentationService : ISchedulerPresentationService
{
    private readonly IEventApi _eventApi;
    private readonly IDateTimeService _dateTimeService;

    public SchedulerPresentationService(
        IEventApi eventApi,
        IDateTimeService dateTimeService)
    {
        _eventApi = eventApi;
        _dateTimeService = dateTimeService;
    }

    public SchedulerIndexViewModel CreateSchedulerIndexViewModel()
    {
        var vm = new SchedulerIndexViewModel();
        return vm;
    }

    public SchedulerModifyViewModel CreateSchedulerCreateViewModel()
    {
        var vm = CreateBaseSchedulerCreateViewModel();
        var today = _dateTimeService.GetDateTimeUtcNow();
        var amountToRound = TimeSpan.FromMinutes(15);

        // Rounds date to the nearest specified minute.
        // "+ amountToRound.Ticks - 1" makes sure today will round up if necessary. E.g. (12 + 5 - 1) = 16, 16 / 5 = 3,  3 * 5 = 15.
        var laterToday = new DateTime(((today.Ticks + amountToRound.Ticks - 1) / amountToRound.Ticks) * amountToRound.Ticks);

        vm.DateFrom = laterToday;
        vm.DateTo = laterToday.AddMinutes(15);
        return vm;
    }

    public SchedulerModifyViewModel CreateSchedulerCreateViewModel(string title, DateTime start, DateTime end)
    {
        var vm = CreateBaseSchedulerCreateViewModel();
        vm.Title = title;
        vm.DateFrom = start;
        vm.DateTo = end;
        return vm;
    }

    public async Task<SchedulerModifyViewModel> CreateSchedulerEditViewModelAsync(Guid id)
    {
        var calendarEvent = await _eventApi.GetEventByIdAsync(id);

        if (calendarEvent == null)
        {
            return null;
        }

        var vm = new SchedulerModifyViewModel();
        vm.AllDay = calendarEvent.AllDay;
        vm.CalendarEventId = calendarEvent.CalendarEventId;
        vm.DateFrom = calendarEvent.DateFrom;
        vm.DateTo = calendarEvent.DateTo;
        vm.Description = calendarEvent.Description;
        vm.Title = calendarEvent.Title;
        vm.ShowDeleteBtn = true;
        vm.ShowExportBtn = true;
        vm.PageTitle = "Edit calendar event";
        return vm;
    }

    public async Task<object> GetCalendarEventsBetweenDateRangeAsync(DateTime start, DateTime end)
    {
        var calEvents = await _eventApi.GetEventsBetweenDateRangeAsync(start, end);
        object result = calEvents.Select(x => new
        {
            title = x.Title,
            start = x.DateFrom.ToString("o"),
            end = x.DateTo.ToString("o"),
            id = x.CalendarEventId,
            allDay = x.AllDay
        });

        return result;
    }

    public async Task<CalendarIcalViewModel> GenerateIcalForCalendarEventAsync(Guid id)
    {
        var calendarEvent = await _eventApi.GetEventByIdAsync(id);

        if (calendarEvent == null)
        {
            return null;
        }

        // Create iCal.
        var iCal = new Ical.Net.Calendar()
        {
            ProductId = "ASP.Net Diary Scheduler",
            Version = "2.0"
        };

        // Create event.
        CreateCalendarIcalEventFromCalendarEvent(iCal, calendarEvent);

        // Build the .ics file.
        return CreateCalendarIcalViewModelFromIcal(iCal);
    }

    public async Task<CalendarIcalViewModel> GenerateIcalBetweenDateRangeAsync(DateTime start, DateTime end)
    {
        // Get user's calendar entries within the date range.
        var calendarEvents = await _eventApi.GetEventsBetweenDateRangeAsync(start, end);

        // Check if there are any diary entries to sync.
        if (calendarEvents == null)
        {
            return null;
        }

        // Create iCal.
        var iCal = new Ical.Net.Calendar()
        {
            ProductId = "ASP.Net Diary Scheduler",
            Version = "2.0"
        };

        // Create a new event for each calendar entry.
        foreach (CalendarEventViewModel calEvent in calendarEvents)
        {
            CreateCalendarIcalEventFromCalendarEvent(iCal, calEvent);
        }

        // Build the .ics file.
        return CreateCalendarIcalViewModelFromIcal(iCal);
    }

    public async Task<Guid> CreateCalendarEventAsync(CalendarEventViewModel eventVm)
    {
        // Save event.
        var id = await _eventApi.CreateEventAsync(eventVm);
        return id;
    }

    public async Task UpdateCalendarEventAsync(CalendarEventViewModel eventVm)
    {
        // Save event.
        await _eventApi.UpdateEventAsync(eventVm.CalendarEventId, eventVm);
    }

    public async Task DeleteCalendarEventAsync(Guid id)
    {
        await _eventApi.DeleteEventAsync(id);
    }

    #region Helpers

    /// <summary>
    /// Create a prepopulated <see cref="SchedulerModifyViewModel"/> for the create variant.
    /// </summary>
    /// <returns>The <see cref="SchedulerModifyViewModel"/>.</returns>
    private SchedulerModifyViewModel CreateBaseSchedulerCreateViewModel()
    {
        var vm = new SchedulerModifyViewModel();
        vm.PageTitle = "Create calendar event";
        return vm;
    }

    /// <summary>
    /// Create a calendar event in the <see cref="Ical.Net.Calendar"/>.
    /// </summary>
    /// <param name="iCal">The calendar to add the event to.</param>
    /// <param name="calendarEvent">The event to add.</param>
    private void CreateCalendarIcalEventFromCalendarEvent(Ical.Net.Calendar iCal, CalendarEventViewModel calendarEvent)
    {
        // Create event.
        var evt = iCal.Create<Ical.Net.CalendarComponents.CalendarEvent>();

        // Prepare ical event.
        evt.Uid = calendarEvent.CalendarEventId.ToString();
        evt.Start = new Ical.Net.DataTypes.CalDateTime(calendarEvent.DateFrom);
        evt.End = new Ical.Net.DataTypes.CalDateTime(calendarEvent.DateTo);
        evt.Description = calendarEvent.Description;
        evt.Summary = calendarEvent.Title;
        evt.IsAllDay = calendarEvent.AllDay;
    }

    /// <summary>
    /// Create the <see cref="CalendarIcalViewModel"/> using the <see cref="Ical.Net.Calendar"/>.
    /// </summary>
    /// <param name="iCal">The calendar to generate an ics from.</param>
    /// <returns>THe <see cref="CalendarIcalViewModel"/> holding the ics data.</returns>
    private CalendarIcalViewModel CreateCalendarIcalViewModelFromIcal(Ical.Net.Calendar iCal)
    {
        var fileData = new CalendarIcalViewModel();
        Ical.Net.Serialization.SerializationContext ctx = new Ical.Net.Serialization.SerializationContext();
        var serialiser = new Ical.Net.Serialization.CalendarSerializer(ctx);

        string output = serialiser.SerializeToString(iCal);
        fileData.ContentType = "text/calendar";
        fileData.Data = System.Text.Encoding.UTF8.GetBytes(output);

        // Create a name.
        Guid fileId = Guid.NewGuid();
        fileData.FileName = fileId.ToString() + ".ics";
        return fileData;
    }

    #endregion
}
