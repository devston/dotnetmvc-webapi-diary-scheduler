import $ from "jquery";
import moment from "moment";
import "fullcalendar";
import "fullcalendar/dist/fullcalendar.min.css";
import { VisibilityHelpers } from "Scripts/Utilities/visibility-helpers";

export namespace SiteCalendar {
    /**
     * Initialise a calendar.
     * @param calendarSelector The calendar selector.
     */
    export function init(calendarSelector: string, sourceUrl: string, createFunc: Function) {
        $(calendarSelector).fullCalendar({
            //eventClick: function (calEvent) {
            //    showEditEntryPanel(calEvent.id);
            //},
            eventLimit: true,
            eventSources: [{
                url: sourceUrl,
                error: function () {
                    VisibilityHelpers.alert("danger", "There was an error while fetching entries.", true);
                }
            }],
            eventDataTransform: function (eventData) {
                // Convert the UTC date to the user's local time.
                return {
                    "id": eventData.id,
                    "title": eventData.title,
                    "allDay": eventData.allDay,
                    "start": moment.utc(eventData.start).local(),
                    "end": moment.utc(eventData.end).local(),
                    "className": eventData.className
                };
            },
            selectable: true,
            selectHelper: true,
            select: function (start: moment.Moment, end: moment.Moment) {
                createFunc(start, end);
            },
            timeFormat: "HH:mm"
        });
    }

    /**
     * Add a calendar event to the calendar.
     * @param eventData
     * @param calendarSelector
     */
    export function addEvent(eventData: any, calendarSelector: string) {
        var event = {
            "id": eventData.id,
            "title": eventData.title,
            "start": moment(eventData.start),
            "end": moment(eventData.end),
            "allDay": eventData.allDay,
            "className": eventData.className
        };

        $(calendarSelector).fullCalendar("renderEvent", event);
    }
}