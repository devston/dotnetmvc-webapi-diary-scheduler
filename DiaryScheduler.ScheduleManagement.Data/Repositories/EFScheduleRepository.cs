using DiaryScheduler.Data.Data;
using DiaryScheduler.Data.Models;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiaryScheduler.ScheduleManagement.Data.Repositories
{
    /// <summary>
    /// The entity framework implementation of the <see cref="IScheduleRepository"/>.
    /// </summary>
    public class EFScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public EFScheduleRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        #region Gets

        public List<CalEventDm> GetAllUserEvents(string id, DateTime start, DateTime end)
        {
            return _context.CalendarEvents.AsNoTracking()
                .Where(x => x.UserId == id && x.DateFrom >= start && x.DateTo <= end)
                .Select(x => new CalEventDm
                {
                    AllDay = x.AllDay,
                    CalendarEntryId = x.CalendarEventId,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    Description = x.Description,
                    Title = x.Title,
                    UserId = x.UserId
                })
                .ToList();
        }

        public CalEventDm GetCalendarEvent(Guid id)
        {
            return _context.CalendarEvents.AsNoTracking()
                .Where(x => x.CalendarEventId == id)
                .Select(x => new CalEventDm
                {
                    AllDay = x.AllDay,
                    CalendarEntryId = x.CalendarEventId,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    Description = x.Description,
                    Title = x.Title,
                    UserId = x.UserId
                })
                .FirstOrDefault();
        }

        #endregion

        #region Checks

        public bool DoesEventExist(Guid id)
        {
            return _context.CalendarEvents.Any(x => x.CalendarEventId == id);
        }

        #endregion

        #region Create, update and delete

        public Guid CreateCalendarEvent(CalEventDm entry)
        {
            var mappedEntry = ConvertCalendarEventDomainModelToEntity(entry);
            _context.CalendarEvents.Attach(mappedEntry);
            _context.Entry(mappedEntry).State = EntityState.Added;
            _context.SaveChanges();
            return mappedEntry.CalendarEventId;
        }

        public void EditCalendarEvent(CalEventDm entry)
        {
            // Get the original event.
            var originalEntry = _context.CalendarEvents.FirstOrDefault(x => x.CalendarEventId == entry.CalendarEntryId);

            // Double check the event exists.
            if (originalEntry == null)
            {
                throw new Exception("The calendar event could not be found.");
            }

            // Update values.
            originalEntry.Title = entry.Title;
            originalEntry.Description = entry.Description;
            originalEntry.DateFrom = entry.DateFrom;
            originalEntry.DateTo = entry.DateTo;
            originalEntry.AllDay = entry.AllDay;

            // Save changes.
            _context.Entry(originalEntry).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCalendarEvent(Guid id)
        {
            // Get the original event.
            var originalEntry = _context.CalendarEvents.FirstOrDefault(x => x.CalendarEventId == id);

            // Double check the event exists.
            if (originalEntry == null)
            {
                throw new Exception("The calendar event could not be found.");
            }

            // Save changes.
            _context.CalendarEvents.Remove(originalEntry);
            _context.SaveChanges();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Convert a <see cref="CalEventDm"/> to a <see cref="CalendarEvent"/>.
        /// </summary>
        /// <param name="entry">The domain model to convert.</param>
        /// <returns>The converted <see cref="CalendarEvent"/>.</returns>
        private CalendarEvent ConvertCalendarEventDomainModelToEntity(CalEventDm entry)
        {
            var newEntity = new CalendarEvent();
            newEntity.AllDay = entry.AllDay;
            newEntity.CalendarEventId = entry.CalendarEntryId;
            newEntity.DateFrom = entry.DateFrom;
            newEntity.DateTo = entry.DateTo;
            newEntity.Description = entry.Description;
            newEntity.Title = entry.Title;
            newEntity.UserId = entry.UserId;
            return newEntity;
        }

        #endregion
    }
}
