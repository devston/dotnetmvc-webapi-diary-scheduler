using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using DiaryScheduler.Web.Common.Classes;
using DiaryScheduler.Web.Common.Utilities;
using DiaryScheduler.Web.Models;
using DiaryScheduler.Web.Models.Scheduler;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DiaryScheduler.Web.Controllers
{
    [Authorize]
    public class SchedulerController : Controller
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ViewModelMapper _mapper;

        public SchedulerController(IScheduleRepository scheduleRepository, ViewModelMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }

        #region Views

        // GET: Scheduler
        public ActionResult Index()
        {
            // TODO: Add a presentation service to store this logic.
            var vm = new SchedulerIndexViewModel();
            vm.CreateEventUrl = Url.Action(nameof(Create), "Scheduler", null);
            vm.CreateEventMoreOptionsUrl = Url.Action(nameof(CreateMoreOptions), "Scheduler", new { title = "title_placeholder", start = "start_placeholder", end = "end_placeholder" });
            vm.EditEventUrl = Url.Action(nameof(Edit), "Scheduler", new { id = "id_placeholder" });
            return View(vm);
        }

        // GET: Create view.
        public ActionResult Create()
        {
            // TODO: Add a presentation service to store this logic.
            DateTime today = DateTime.UtcNow;
            TimeSpan amountToRound = TimeSpan.FromMinutes(15);

            // Rounds date to the nearest specified minute.
            // "+ amountToRound.Ticks - 1" makes sure today will round up if necessary. E.g. (12 + 5 - 1) = 16, 16 / 5 = 3,  3 * 5 = 15.
            DateTime laterToday = new DateTime(((today.Ticks + amountToRound.Ticks - 1) / amountToRound.Ticks) * amountToRound.Ticks);

            var entry = new CalendarEventViewModel()
            {
                DateFrom = laterToday,
                DateTo = laterToday.AddMinutes(15)
            };

            return View(entry);
        }

        // GET: Create view with filled in options.
        public ActionResult CreateMoreOptions(string title, DateTime start, DateTime end)
        {
            var entry = new CalendarEventViewModel()
            {
                Title = title,
                DateFrom = start,
                DateTo = end
            };

            return View("Create", entry);
        }

        // GET: Edit event view.
        public ActionResult Edit(Guid id)
        {
            // Check if an id was sent.
            if (id == Guid.Empty)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> Invalid calendar event id.", "");
            }

            var entry = _scheduleRepository.GetCalendarEntry(id);

            if (entry == null)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> The calendar event could not be found.", "");
            }

            var vm = _mapper.Map<CalendarEventViewModel>(entry);
            return View(vm);
        }

        // GET: Quick create modal.
        public ActionResult _ModalQuickCreate()
        {
            return PartialView();
        }

        // GET: Quick create view.
        public ActionResult _QuickCreate(DateTime start, DateTime end)
        {
            var vm = new CalendarEventViewModel()
            {
                DateFrom = start,
                DateTo = end
            };
            return PartialView(vm);
        }

        // GET: Export calendar events modal.
        public ActionResult _ModalExport()
        {
            return PartialView();
        }

        // GET: Confirm event deletion modal.
        public ActionResult _ModalDeleteConfirmation()
        {
            return PartialView();
        }

        #endregion

        #region Posts

        // Create calendar entry.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEntry(CalendarEventViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> Input is invalid.", "");
            }

            // Date range check.
            if (vm.DateFrom > vm.DateTo)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> Start date cannot be after the end date.", "");
            }

            var entry = new CalEntryDm()
            {
                Title = vm.Title.Trim(),
                Description = vm.Description == null ? null : vm.Description.Trim(),
                DateFrom = vm.DateFrom,
                DateTo = vm.DateTo,
                AllDay = vm.AllDay,
                UserId = User.Identity.GetUserId()
            };

            // Save event.
            var id = _scheduleRepository.CreateCalendarEntry(entry);

            // Return calendar record for fullCalendar.js.
            return Json(new
            {
                message = "<strong>Success:</strong> Calendar event created.",
                calEntry = new
                {
                    id,
                    title = entry.Title,
                    start = entry.DateFrom,
                    end = entry.DateTo,
                    allDay = entry.AllDay
                },
                backUrl = Url.Action(nameof(Index), "Scheduler", null)
            });
        }

        // Edit calendar entry.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEntry(CalendarEventViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> Input is invalid.", "");
            }

            // Date range check.
            if (vm.DateFrom > vm.DateTo)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> Start date cannot be after the end date.", "");
            }

            // Check if the calendar entry exists.
            if (!_scheduleRepository.DoesCalEntryExist(vm.CalendarEntryId))
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> The calendar event could not be found.", "");
            }

            var entry = _mapper.Map<CalEntryDm>(vm);

            // Save event.
            _scheduleRepository.EditCalendarEntry(entry);

            return Json(new
            {
                message = "<strong>Success:</strong> Calendar event saved.",
                backUrl = Url.Action(nameof(Index), "Scheduler", null)
            });
        }

        // Delete calendar entry.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEntry(Guid id)
        {
            // Check if the calendar entry exists.
            if (!_scheduleRepository.DoesCalEntryExist(id))
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> The calendar event could not be found.", "");
            }

            // Delete event.
            _scheduleRepository.DeleteCalendarEntry(id);

            return Json(new
            {
                message = "<strong>Success:</strong> Calendar entry deleted.",
                backUrl = Url.Action(nameof(Index), "Scheduler", null)
            });
        }

        #endregion

        #region Calendar Sources

        // GET: User calendar entries.
        public ActionResult UserEntries(DateTime start, DateTime end)
        {
            var userId = User.Identity.GetUserId();
            List<CalEntryDm> userEntries = _scheduleRepository.GetAllUserEntries(userId, start, end);

            var result = userEntries.Select(x => new
            {
                title = x.Title,
                start = x.DateFrom.ToString("o"),
                end = x.DateTo.ToString("o"),
                id = x.CalendarEntryId,
                allDay = x.AllDay
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Export

        // Create a .ics file for a calendar event.
        public ActionResult ExportEventToIcal(Guid id)
        {
            // Check if an id was sent.
            if (id == Guid.Empty)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> Invalid calendar event id.", "");
            }

            var entry = _scheduleRepository.GetCalendarEntry(id);

            if (entry == null)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> The calendar event could not be found.", "");
            }

            // Create iCal.
            var iCal = new Ical.Net.Calendar()
            {
                ProductId = "ASP.Net Diary Scheduler",
                Version = "2.0"
            };

            // Create event.
            var evt = iCal.Create<Ical.Net.CalendarComponents.CalendarEvent>();

            // Prepare ical event.
            evt.Uid = entry.CalendarEntryId.ToString();
            evt.Start = new Ical.Net.DataTypes.CalDateTime(entry.DateFrom);
            evt.End = new Ical.Net.DataTypes.CalDateTime(entry.DateTo);
            evt.Description = entry.Description;
            evt.Summary = entry.Title;
            evt.IsAllDay = entry.AllDay;

            // Build the .ics file.
            var ctx = new Ical.Net.Serialization.SerializationContext();
            var serialiser = new Ical.Net.Serialization.CalendarSerializer(ctx);

            string output = serialiser.SerializeToString(iCal);
            string contentType = "text/calendar";
            var bytes = System.Text.Encoding.UTF8.GetBytes(output);

            // Create a name.
            Guid fileId = Guid.NewGuid();
            string fileName = fileId.ToString() + ".ics";

            return File(bytes, contentType, fileName);
        }

        // Create a .ics file for calendar events from a date range.
        public ActionResult ExportEventsToIcal(DateTime start, DateTime end)
        {
            // Check if dates are null before doing anything.
            if (start == null || end == null)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> No date range provided.", "");
            }

            // Get user's calendar entries within the date range.
            List<CalEntryDm> userEntries = _scheduleRepository.GetAllUserEntries(User.Identity.GetUserId(), start, end);

            // Check if there are any diary entries to sync.
            if (userEntries == null)
            {
                return SiteErrorHandler.GetBadRequestActionResult("<strong>Error:</strong> No calendar events to sync.", "");
            }

            // Create iCal.
            var iCal = new Ical.Net.Calendar()
            {
                ProductId = "ASP.Net Diary Scheduler",
                Version = "2.0"
            };

            // Create a new event for each calendar entry.
            foreach (CalEntryDm entry in userEntries)
            {
                // Create event.
                var evt = iCal.Create<Ical.Net.CalendarComponents.CalendarEvent>();

                // Prepare ical event.
                evt.Uid = entry.CalendarEntryId.ToString();
                evt.Start = new Ical.Net.DataTypes.CalDateTime(entry.DateFrom);
                evt.End = new Ical.Net.DataTypes.CalDateTime(entry.DateTo);
                evt.Description = entry.Description;
                evt.Summary = entry.Title;
                evt.IsAllDay = entry.AllDay;
            }

            // Build the .ics file.
            Ical.Net.Serialization.SerializationContext ctx = new Ical.Net.Serialization.SerializationContext();
            var serialiser = new Ical.Net.Serialization.CalendarSerializer(ctx);

            string output = serialiser.SerializeToString(iCal);
            string contentType = "text/calendar";
            var bytes = System.Text.Encoding.UTF8.GetBytes(output);

            // Create a name.
            Guid fileId = Guid.NewGuid();
            string fileName = fileId.ToString() + ".ics";

            return File(bytes, contentType, fileName);
        }

        #endregion
    }
}