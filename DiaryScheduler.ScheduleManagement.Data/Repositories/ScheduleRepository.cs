using DiaryScheduler.Data.Data;
using DiaryScheduler.Data.Models;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using DiaryScheduler.ScheduleManagement.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DiaryScheduler.ScheduleManagement.Data.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DomainMapperService _mapper;

        public ScheduleRepository(DomainMapperService mapper)
        {
            _context = new ApplicationDbContext();
            _mapper = mapper;
        }

        #region Gets

        // Return all user calendar entries.
        public List<CalEntry> GetAllUserEntries(string id, DateTime start, DateTime end)
        {
            return _mapper.Map<List<CalEntry>>(
                    _context.CalendarEntries.AsNoTracking().Where(x => x.UserId == id && x.DateFrom >= start && x.DateTo <= end)
                );
        }

        #endregion

        #region Create, update and delete

        // Add new calendar entry.
        public Guid CreateCalendarEntry(CalEntry entry)
        {
            var mappedEntry = _mapper.Map<CalendarEntry>(entry);
            _context.CalendarEntries.Attach(mappedEntry);
            _context.Entry(mappedEntry).State = EntityState.Added;
            SaveChanges();
            return mappedEntry.CalendarEntryId;
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
