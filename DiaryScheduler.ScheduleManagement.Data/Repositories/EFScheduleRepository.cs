using DiaryScheduler.Data.Data;
using DiaryScheduler.Data.Models;
using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<List<CalEventDm>> GetAllEventsBetweenDateRangeAsync(DateTime start, DateTime end)
        {
            var query = SelectCalendarEventDmFromQuery(
                    _context.CalendarEvents.AsNoTracking()
                    .Where(x => x.DateFrom >= start && x.DateTo <= end)
                );
            return await query.ToListAsync();
        }

        public async Task<CalEventDm> GetCalendarEventByEventIdAsync(Guid id)
        {
            var query = SelectCalendarEventDmFromQuery(
                    _context.CalendarEvents.AsNoTracking()
                    .Where(x => x.CalendarEventId == id)
                );
            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region Checks

        public async Task<bool> DoesEventExistAsync(Guid id)
        {
            return await _context.CalendarEvents.AnyAsync(x => x.CalendarEventId == id);
        }

        #endregion

        #region Create, update and delete

        public async Task<Guid> CreateCalendarEventAsync(CalEventDm entry)
        {
            var mappedEntry = ConvertCalendarEventDomainModelToEntity(entry);
            _context.CalendarEvents.Attach(mappedEntry);
            _context.Entry(mappedEntry).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return mappedEntry.CalendarEventId;
        }

        public async Task EditCalendarEventAsync(CalEventDm entry)
        {
            // Get the original event.
            var originalEntry = await _context.CalendarEvents.FirstOrDefaultAsync(x => x.CalendarEventId == entry.CalendarEntryId);

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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCalendarEventAsync(Guid id)
        {
            // Get the original event.
            var originalEntry = await _context.CalendarEvents.FirstOrDefaultAsync(x => x.CalendarEventId == id);

            // Double check the event exists.
            if (originalEntry == null)
            {
                throw new Exception("The calendar event could not be found.");
            }

            // Save changes.
            _context.CalendarEvents.Remove(originalEntry);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Add a .Select transforming <see cref="CalendarEvent"/> to <see cref="CalEventDm"/>.
        /// </summary>
        /// <param name="query">The query to transform.</param>
        /// <returns>An <see cref="IQueryable"/> of <see cref="CalEventDm"/>.</returns>
        private IQueryable<CalEventDm> SelectCalendarEventDmFromQuery(IQueryable<CalendarEvent> query)
        {
            return query.Select(x => new CalEventDm
            {
                AllDay = x.AllDay,
                CalendarEntryId = x.CalendarEventId,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                Description = x.Description,
                Title = x.Title
            });
        }

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
            return newEntity;
        }

        #endregion
    }
}
