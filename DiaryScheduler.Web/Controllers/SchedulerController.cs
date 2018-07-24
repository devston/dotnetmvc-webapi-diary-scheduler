using DiaryScheduler.ScheduleManagement.Core.Interfaces;
using DiaryScheduler.ScheduleManagement.Core.Models;
using DiaryScheduler.Web.Common.Classes;
using DiaryScheduler.Web.Common.Utilities;
using DiaryScheduler.Web.Models;
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
            return View();
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

            var entry = new CalEntry()
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
                success = true,
                message = "<strong>Success:</strong> Calendar entry created.",
                calEntry = new
                {
                    id = id,
                    title = entry.Title,
                    start = entry.DateFrom,
                    end = entry.DateTo,
                    allDay = entry.AllDay
                }
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Calendar Sources

        // GET: User calendar entries.
        public ActionResult UserEntries(DateTime start, DateTime end)
        {
            var userId = User.Identity.GetUserId();
            List<CalEntry> userEntries = _scheduleRepository.GetAllUserEntries(userId, start, end);

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
    }
}