using DiaryScheduler.Data.Data;
using DiaryScheduler.Data.Models;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using DiaryScheduler.ScheduleManagement.Data.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public EFScheduleRepository(DomainMapperService mapper)
        {
            _context = new ApplicationDbContext();
            _mapper = mapper;
        }

        #region Gets

        // Return all user calendar entries.
        public List<CalEntryDm> GetAllUserEntries(string id, DateTime start, DateTime end)
        {
            return _mapper.Map<List<CalEntryDm>>(
                    _context.CalendarEntries.AsNoTracking().Where(x => x.UserId == id && x.DateFrom >= start && x.DateTo <= end)
                );
        }

        // Get a calendar entry by id.
        public CalEntryDm GetCalendarEntry(Guid id)
        {
            return _mapper.Map<CalEntryDm>(
                    _context.CalendarEntries.AsNoTracking().FirstOrDefault(x => x.CalendarEntryId == id)
                );
        }

        #endregion

        #region Checks

        // Check if the calendar entry exists.
        public bool DoesCalEntryExist(Guid id)
        {
            return _context.CalendarEntries.Any(x => x.CalendarEntryId == id);
        }

        #endregion

        #region Create, update and delete

        // Add new calendar entry.
        public Guid CreateCalendarEntry(CalEntryDm entry)
        {
            var mappedEntry = _mapper.Map<CalendarEntry>(entry);
            _context.CalendarEntries.Attach(mappedEntry);
            _context.Entry(mappedEntry).State = EntityState.Added;
            SaveChanges();
            return mappedEntry.CalendarEntryId;
        }

        // Edit an existing calendar entry.
        public void EditCalendarEntry(CalEntryDm entry)
        {
            // Get the original entry.
            var originalEntry = _context.CalendarEntries.FirstOrDefault(x => x.CalendarEntryId == entry.CalendarEntryId);

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
            var originalEntry = _context.CalendarEntries.FirstOrDefault(x => x.CalendarEntryId == id);

            // Double check the entry exists.
            if (originalEntry == null)
            {
                throw new Exception("The calendar entry could not be found.");
            }

            // Save changes.
            _context.CalendarEntries.Remove(originalEntry);
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
