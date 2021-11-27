using DiaryScheduler.Data.Data;
using DiaryScheduler.Data.Models;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using DiaryScheduler.ScheduleManagement.Data.Services;
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
        private readonly DomainMapperService _mapper;

        public EFScheduleRepository(
            ApplicationDbContext context,
            DomainMapperService mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Gets

        // Return all user calendar entries.
        public List<CalEventDm> GetAllUserEntries(string id, DateTime start, DateTime end)
        {
            return _mapper.Map<List<CalEventDm>>(
                    _context.CalendarEvents.AsNoTracking().Where(x => x.UserId == id && x.DateFrom >= start && x.DateTo <= end)
                );
        }

        // Get a calendar entry by id.
        public CalEventDm GetCalendarEntry(Guid id)
        {
            return _mapper.Map<CalEventDm>(
                    _context.CalendarEvents.AsNoTracking().FirstOrDefault(x => x.CalendarEventId == id)
                );
        }

        #endregion

        #region Checks

        // Check if the calendar entry exists.
        public bool DoesCalEntryExist(Guid id)
        {
            return _context.CalendarEvents.Any(x => x.CalendarEventId == id);
        }

        #endregion

        #region Create, update and delete

        // Add new calendar entry.
        public Guid CreateCalendarEntry(CalEventDm entry)
        {
            var mappedEntry = _mapper.Map<CalendarEvent>(entry);
            _context.CalendarEvents.Attach(mappedEntry);
            _context.Entry(mappedEntry).State = EntityState.Added;
            SaveChanges();
            return mappedEntry.CalendarEventId;
        }

        // Edit an existing calendar entry.
        public void EditCalendarEntry(CalEventDm entry)
        {
            // Get the original entry.
            var originalEntry = _context.CalendarEvents.FirstOrDefault(x => x.CalendarEventId == entry.CalendarEntryId);

            // Double check the entry exists.
            if (originalEntry == null)
            {
                throw new Exception("The calendar entry could not be found.");
            }

            // Update values.
            originalEntry.Title = entry.Title;
            originalEntry.Description = entry.Description;
            originalEntry.DateFrom = entry.DateFrom;
            originalEntry.DateTo = entry.DateTo;
            originalEntry.AllDay = entry.AllDay;

            // Save changes.
            _context.Entry(originalEntry).State = EntityState.Modified;
            SaveChanges();
        }

        // Delete a calendar entry.
        public void DeleteCalendarEntry(Guid id)
        {
            // Get the original entry.
            var originalEntry = _context.CalendarEvents.FirstOrDefault(x => x.CalendarEventId == id);

            // Double check the entry exists.
            if (originalEntry == null)
            {
                throw new Exception("The calendar entry could not be found.");
            }

            // Save changes.
            _context.CalendarEvents.Remove(originalEntry);
            SaveChanges();
        }

        #endregion

        #region Helpers

        private void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion
    }
}
