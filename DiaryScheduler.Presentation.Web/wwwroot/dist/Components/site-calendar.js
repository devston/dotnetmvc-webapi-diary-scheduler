import { Calendar } from "@fullcalendar/core";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import listPlugin from "@fullcalendar/list";
import interactionPlugin from "@fullcalendar/interaction";
export var SiteCalendar;
(function (SiteCalendar) {
    /**
     *  An object to store any calendar instances.
     * */
    var calendars = {};
    /**
     * Initialise a calendar.
     * @param calendarSelector The calendar selector.
     */
    function init(calendarSelector, sourceUrl, createFunc, editFunc) {
        var calendarEl = document.querySelector(calendarSelector);
        var calendar = new Calendar(calendarEl, {
            plugins: [dayGridPlugin, timeGridPlugin, listPlugin, interactionPlugin],
            initialView: "dayGridMonth",
            headerToolbar: {
                left: "prev,next today",
                center: "title",
                right: "dayGridMonth,timeGridWeek,listWeek"
            },
            eventClick: function (calEvent) {
                editFunc(calEvent.event.id);
            },
            dayMaxEventRows: true,
            events: sourceUrl,
            selectable: true,
            select: function (info) {
                console.log("click");
                createFunc(info.start, info.end);
            }
        });
        calendars[calendarSelector] = calendar;
        calendar.render();
    }
    SiteCalendar.init = init;
    /**
     * Get the visible date range on the calendar.
     * @param calendarSelector
     */
    function getVisibleDates(calendarSelector) {
        var view = calendars[calendarSelector].view;
        var dates = {
            start: view.intervalStart.format(),
            end: view.intervalEnd.format()
        };
        return dates;
    }
    SiteCalendar.getVisibleDates = getVisibleDates;
    /**
     * Add a calendar event to the calendar.
     * @param eventData
     * @param calendarSelector
     */
    function addEvent(eventData, calendarSelector) {
        var event = {
            "id": eventData.id,
            "title": eventData.title,
            "start": eventData.start,
            "end": eventData.end,
            "allDay": eventData.allDay,
            "className": eventData.className
        };
        calendars[calendarSelector].addEvent(event);
    }
    SiteCalendar.addEvent = addEvent;
})(SiteCalendar || (SiteCalendar = {}));
//# sourceMappingURL=site-calendar.js.map