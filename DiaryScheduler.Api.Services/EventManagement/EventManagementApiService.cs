using DiaryScheduler.Presentation.Models.Scheduler;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiaryScheduler.Api.Services.EventManagement
{
    /// <summary>
    /// The implementation of the <see cref="IEventManagementApiService"/>.
    /// </summary>
    public class EventManagementApiService : IEventManagementApiService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public EventManagementApiService(
            IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<List<CalendarEventViewModel>> GetCalendarEventsBetweenDateRangeAsync(DateTime start, DateTime end)
        {
            var calEvents = await _scheduleRepository.GetAllEventsBetweenDateRangeAsync(start, end);

            if (calEvents == null)
            {
                return new List<CalendarEventViewModel>();
            }

            return calEvents.Select(x => ConvertCalendarEventDomainModelToViewModel(x)).ToList();
        }

        public async Task<CalendarEventViewModel> GetCalendarEventByIdAsync(Guid id)
        {
            var calendarEvent = await _scheduleRepository.GetCalendarEventByEventIdAsync(id);

            if (calendarEvent == null)
            {
                throw new NullReferenceException("The calendar event could not be found.");
            }

            return ConvertCalendarEventDomainModelToViewModel(calendarEvent);
        }

        public async Task<Guid> CreateCalendarEventAsync(CalendarEventViewModel eventVm)
        {
            var calEvent = ConvertCalendarEventViewModelToDomainModel(eventVm);

            // Save event.
            var id = await _scheduleRepository.CreateCalendarEventAsync(calEvent);
            return id;
        }

        public async Task<string> UpdateCalendarEventAsync(CalendarEventViewModel eventVm)
        {
            if (!await _scheduleRepository.DoesEventExistAsync(eventVm.CalendarEventId))
            {
                throw new Exception("The calendar event could not be found.");
            }

            var calEvent = ConvertCalendarEventViewModelToDomainModel(eventVm);

            // Save event.
            await _scheduleRepository.EditCalendarEventAsync(calEvent);
            return "The calendar event was updated.";
        }

        public async Task<string> DeleteCalendarEventAsync(Guid id)
        {
            if (!await _scheduleRepository.DoesEventExistAsync(id))
            {
                throw new Exception("The calendar event could not be found.");
            }

            await _scheduleRepository.DeleteCalendarEventAsync(id);
            return "The calendar event was deleted.";
        }

        /// <summary>
        /// Convert a <see cref="CalEventDm"/> to a <see cref="CalendarEventViewModel"/>.
        /// </summary>
        /// <param name="eventDm">The model to convert.</param>
        /// <returns>The converted <see cref="CalendarEventViewModel"/>.</returns>
        private CalendarEventViewModel ConvertCalendarEventDomainModelToViewModel(CalEventDm eventDm)
        {
            var vm = new CalendarEventViewModel();
            vm.AllDay = eventDm.AllDay;
            vm.CalendarEventId = eventDm.CalendarEntryId;
            vm.DateFrom = eventDm.DateFrom;
            vm.DateTo = eventDm.DateTo;
            vm.Description = eventDm.Description;
            vm.Title = eventDm.Title;
            return vm;
        }

        /// <summary>
        /// Convert a <see cref="CalendarEventViewModel"/> to a <see cref="CalEventDm"/>.
        /// </summary>
        /// <param name="eventVm">The model to convert.</param>
        /// <returns>The converted <see cref="CalEventDm"/>.</returns>
        private CalEventDm ConvertCalendarEventViewModelToDomainModel(CalendarEventViewModel eventVm)
        {
            var calEvent = new CalEventDm()
            {
                CalendarEntryId = eventVm.CalendarEventId,
                Title = eventVm.Title.Trim(),
                Description = string.IsNullOrEmpty(eventVm.Description) ? null : eventVm.Description.Trim(),
                DateFrom = eventVm.DateFrom,
                DateTo = eventVm.DateTo,
                AllDay = eventVm.AllDay
            };
            return calEvent;
        }
    }
}
