/*----------------------------------------------------------------------------*\
    
    Scheduler
    ---------

    All js related to the scheduler area is found here.

    Contents
    --------

    1. Initialisation functions
    2. Create, update and delete functions
    3. Helper functions
    4. Navigation functions
    
\*----------------------------------------------------------------------------*/

import $ from "jquery";
import moment from "moment";
import "fullcalendar";
import { Site } from "Scripts/Utilities/site-core";
import { VisibilityHelpers } from "Scripts/Utilities/visibility-helpers";

/**
 * A module containing all the logic for the scheduler area.
 */
export namespace Scheduler {

    /*------------------------------------------------------------------------*\
        1. Initialisation functions
    \*------------------------------------------------------------------------*/

    /**
     * Initialise scheduler area.
     */
    export function init(pageToLoad: string) {
        switch (pageToLoad) {
            default:
                initIndex();
                break;
        }
    }

    /**
     * Initialise the index page.
     */
    function initIndex() {
        initCalendar("#calendar");
    }

    /*------------------------------------------------------------------------*\
        2. Create, update and delete functions
    \*------------------------------------------------------------------------*/

    /**
     *  Create a new calendar entry from the quick create modal.
     * */
    function quickCreate() {
        // jQuery object is used multiple times so store it in a variable.
        var $form = $("#quick-create-form");

        if ($form.valid()) {
            $.ajax({
                beforeSend: function () {
                    VisibilityHelpers.loader(true);
                },
                url: "/Scheduler/CreateEntry",
                type: "POST",
                data: $form.serialize()
            }).always(function () {
                VisibilityHelpers.loader(false);
            }).done(function (data: any) {
                addEntryToCalendar(data.calEntry);
                VisibilityHelpers.alert("success", data.message, true);

                // jQuery object is used multiple times so store it in a variable.
                var $modal = $("#quick-create-modal");

                // Close and empty the modal.
                $modal.modal("hide");

                $modal.on("hidden.bs.modal", function () {
                    $("#quick-create-container").empty();
                });
            })
            .fail(function (jqXHR) {
                Site.showJqXhrAsAlert(jqXHR);
            });
        }
    }

    /*------------------------------------------------------------------------*\
        3. Helper functions
    \*------------------------------------------------------------------------*/

    /**
     * Initialise the calendar.
     * @param calendarSelector
     */
    function initCalendar(calendarSelector: string) {
        $(calendarSelector).fullCalendar({
            //eventClick: function (calEvent) {
            //    showEditEntryPanel(calEvent.id);
            //},
            eventLimit: true,
            eventSources: [{
                url: "/Scheduler/UserEntries",
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
                showQuickCreateModal(start, end);
            },
            timeFormat: "HH:mm"
        });
    }

    // Show quick create modal.
    function showQuickCreateModal(start: moment.Moment, end: moment.Moment) {
        VisibilityHelpers.loader(true);

        // Format moment object to ISO 8601 so no date conversion errors are made on model bind.
        var startFormatted = start.format("YYYY-MM-DD HH:mm");
        var endFormatted = end.format("YYYY-MM-DD HH:mm");

        // jQuery object is used multiple times so store it in a variable.
        var $container = $("#quick-create-container");

        // Open create panel.
        $container.load("/Scheduler/_QuickCreate/", { "start": startFormatted, "end": endFormatted }, function (_response: string, status: string) {
            if (status == "success") {
                // Format date so it's human readable.
                $("#DateStarting").val(start.local().format("LLL"));
                $("#DateEnding").val(end.local().format("LLL"));

                // jQuery objects are used multiple times so store them in variables.
                var $modal = $("#quick-create-modal");
                var $form = $("#quick-create-form");

                // Show the modal.
                $modal.modal("show");

                // Show full create view.
                $("#edit-entry-btn").on("click", function (event: JQueryEventObject) {
                    event.preventDefault();

                    VisibilityHelpers.loader(true);
                    var title = <string>$("#Title").val();

                    // Close and empty the modal.
                    $modal.modal("hide");

                    Navigate.toQuickCreate(title, startFormatted, endFormatted);

                    VisibilityHelpers.loader(false);
                });

                // Submit form.
                $form.on("submit", function (event: JQueryEventObject) {
                    event.preventDefault();
                    quickCreate();
                });

                VisibilityHelpers.loader(false);
            }
        });
    }

    /**
     *  Add a calendar entry to the calendar.
     * */
    function addEntryToCalendar(eventData: any) {
        var event = {
            "id": eventData.id,
            "title": eventData.title,
            "start": moment(eventData.start),
            "end": moment(eventData.end),
            "allDay": eventData.allDay,
            "className": eventData.className
        };

        $("#calendar").fullCalendar("renderEvent", event);
    }

    /*------------------------------------------------------------------------*\
        4. Navigation functions
    \*------------------------------------------------------------------------*/

    /**
     * Contains navigation based functionality for the scheduler area.
     */
    namespace Navigate {
        /**
         * Navigate to the scheduler index page.
         */
        export function toIndex() {
            Site.loadPartial("/Scheduler/");
        }

        /**
         * Navigate to the quick entry create page.
         * @param title
         * @param start
         * @param end
         */
        export function toQuickCreate(title: string, start: string, end: string) {
            Site.loadPartial("/Scheduler/QuickCreateEntry/", { "title": title, "start": start, "end": end })
        }
    }
}