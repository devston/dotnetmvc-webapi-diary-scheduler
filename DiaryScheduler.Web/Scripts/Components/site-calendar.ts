import { Calendar } from "@fullcalendar/core";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import listPlugin from "@fullcalendar/list";
import interactionPlugin from "@fullcalendar/interaction";

export namespace SiteCalendar {
    /**
     *  An object to store any calendar instances.
     * */
    let calendars = {};

    /**
     * Initialise a calendar.
     * @param calendarSelector The calendar selector.
     */
    export function init(calendarSelector: string, sourceUrl: string, createFunc: Function, editFunc: Function) {
        const calendarEl = document.querySelector(calendarSelector) as HTMLElement;
        let calendar = new Calendar(calendarEl, {
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
            eventSources: [{
                id: "default",
                url: sourceUrl
            }],
            selectable: true,
            //selectMirror: true,
            select: function (info) {
                console.log("click");
                createFunc(info.start, info.end);
            }
        });

        calendars[calendarSelector] = calendar;
        calendar.render();
    }

    /**
     * Get the visible date range on the calendar.
     * @param calendarSelector
     */
    export function getVisibleDates(calendarSelector: string) {
        const view = calendars[calendarSelector].view;
        const dates = {
            start: view.intervalStart.format(),
            end: view.intervalEnd.format()
        };

        return dates;
    }

    /**
     * Add a calendar event to the calendar.
     * @param eventData
     * @param calendarSelector
     */
    export function addEvent(eventData: any, calendarSelector: string) {
        const event = {
            "id": eventData.id,
            "title": eventData.title,
            "start": eventData.start,
            "end": eventData.end,
            "allDay": eventData.allDay,
            "className": eventData.className
        };

        calendars[calendarSelector].addEvent(event);
    }
}