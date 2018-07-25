/*----------------------------------------------------------------------------*\
    
    Scheduler
    ---------

    All js related to the scheduler area is found here.

    Contents
    --------

    1. Initialisation functions
    2. Create, update and delete functions
    3. Export functions
    4. Helper functions
    5. Navigation functions
    
\*----------------------------------------------------------------------------*/

import $ from "jquery";
import moment from "moment";
import "fullcalendar";
import { Site } from "Scripts/Utilities/site-core";
import { VisibilityHelpers } from "Scripts/Utilities/visibility-helpers";
import { SiteCalendar } from "Scripts/Components/site-calendar";
import { DateTimePicker } from "Scripts/Components/site-datetimepicker";

/**
 * A module containing all the logic for the scheduler area.
 */
export namespace Scheduler {

    /*------------------------------------------------------------------------*\
        1. Initialisation functions
    \*------------------------------------------------------------------------*/

    const calendarSelector = "#calendar";

    /**
     * Initialise scheduler area.
     */
    export function init(pageToLoad: string) {
        switch (pageToLoad) {
            case "create":
                initCreate();
                break;

            case "edit":
                initEdit();
                break;

            default:
                initIndex();
                break;
        }
    }

    /**
     * Initialise the index page.
     */
    function initIndex() {
        const sourceUrl = "/Scheduler/UserEntries/";
        SiteCalendar.init(calendarSelector, sourceUrl, showQuickCreateModal, Navigate.toEdit);

        $("#create-event-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toCreate();
        });

        $("#export-events-btn").on("click", function (e) {
            e.preventDefault();
            initExportModal();
        });
    }

    /**
     *  Initialise the create page.
     * */
    function initCreate() {
        initEventCard("#create-cal-entry-form", "/Scheduler/CreateEntry/");
    }

    /**
     *  Initialise the edit event page.
     * */
    function initEdit() {
        const eventId = <string>$("#CalendarEntryId").val();
        initEventCard("#edit-cal-entry-form", "/Scheduler/EditEntry/");

        $("#export-event-btn").on("click", function (e) {
            e.preventDefault();
            exportEventToIcal(eventId);
        });

        $("#delete-entry-btn").on("click", function (e) {
            e.preventDefault();
            initDeleteModal(eventId);
        });
    }

    /**
     * Initialise the event card.
     * @param formSelector
     * @param url
     */
    function initEventCard(formSelector: string, url: string) {
        const startPickerSelector = "#DateStarting";
        const endPickerSelector = "#DateEnding";

        // Initialise the datetime pickers.
        DateTimePicker.initDateTime(startPickerSelector);
        DateTimePicker.initDateTime(endPickerSelector);
        DateTimePicker.initRange(startPickerSelector, endPickerSelector);

        // Form submit.
        $(formSelector).on("submit", function (e) {
            e.preventDefault();
            saveEvent($(this), url);
        });

        $("#back-to-cal-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toIndex();
        });
    }

    /**
     *  Initialise the export calendar event modal.
     * */
    function initExportModal() {
        const $modal = $("#export-events-modal");
        const startPickerSelector = "#SyncFrom";
        const endPickerSelector = "#SyncTo";
        let radioVal = "0";

        // Initialise the datetime pickers.
        DateTimePicker.initDateTime(startPickerSelector);
        DateTimePicker.initDateTime(endPickerSelector);
        DateTimePicker.initRange(startPickerSelector, endPickerSelector);

        // Initialise the radio controls.
        $("#calendar-sync-options-container").find("input[name=\"calsync\"]").on("change", function (this: HTMLInputElement) {
            if (this.value == "1") {
                $("#date-sync-container").addClass("hidden");
                $("#calendar-sync-container").removeClass("hidden");
            }
            else {
                $("#calendar-sync-container").addClass("hidden");
                $("#date-sync-container").removeClass("hidden");
            }

            radioVal = this.value;
        });

        $("#confirm-export-btn").on("click", function (e) {
            e.preventDefault();

            // No value selected.
            if (radioVal == "0") {
                return;
            }
            else {
                $modal.modal("hide");

                if (radioVal == "1") {
                    exportVisibleEventsToIcal();
                }
                else if (radioVal == "2") {
                    var start = <string>$("#SyncFromHid").val();
                    var end = <string>$("#SyncToHid").val();

                    exportEventsFromDateRangeToIcal(start, end);
                }
            }
        });

        $modal.modal("show");
    }

    /**
     *  Initialise the delete event modal.
     * */
    function initDeleteModal(eventId: string) {
        const $modal = $("#confirm-delete-modal");

        $("#confirm-delete-btn").on("click", function (e) {
            e.preventDefault();
            $modal.modal("hide");

            // Stop the modal from getting 'stuck'.
            $modal.on("hidden.bs.modal", function () {
                deleteEvent(eventId);
            });
        });

        $modal.modal("show");
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
                    $("#edit-entry-btn").attr("disabled");
                    $("#quick-create-btn").attr("disabled");
                },
                url: "/Scheduler/CreateEntry/",
                type: "POST",
                data: $form.serialize()
            })
            .always(function () {
                VisibilityHelpers.loader(false);
                $("#edit-entry-btn").removeAttr("disabled");
                $("#quick-create-btn").removeAttr("disabled");
            })
            .done(function (data: any) {
                SiteCalendar.addEvent(data.calEntry, calendarSelector);
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

    /**
     * Save a calendar event.
     * @param formId
     * @param url
     */
    function saveEvent($form: JQuery<HTMLElement>, url: string) {
        if ($form.valid()) {
            $.ajax({
                beforeSend: function () {
                    VisibilityHelpers.loader(true);
                    $("#save-entry-btn").attr("disabled");
                    $("#delete-entry-btn").attr("disabled");
                    $("#back-to-cal-btn").attr("disabled");
                },
                url: url,
                type: "POST",
                data: $form.serialize()
            }).always(function () {
                VisibilityHelpers.loader(false);
                $("#save-entry-btn").removeAttr("disabled");
                $("#delete-entry-btn").removeAttr("disabled");
                $("#back-to-cal-btn").removeAttr("disabled");
            })
            .done(function (data: any) {
                VisibilityHelpers.alert("success", data.message, true);
                Navigate.toIndex();
            })
            .fail(function (jqXHR) {
                Site.showJqXhrAsAlert(jqXHR);
            });
        }
    }

    /**
     *  Delete a calendar event.
     * */
    function deleteEvent(eventId: string) {
        const token = <string>$("#edit-cal-entry-form").find("input[name=__RequestVerificationToken]").val();

        $.ajax({
            beforeSend: function () {
                VisibilityHelpers.loader(true);
                $("#save-entry-btn").attr("disabled");
                $("#delete-entry-btn").attr("disabled");
                $("#back-to-cal-btn").attr("disabled");
            },
            url: "/Scheduler/DeleteEntry/",
            type: "POST",
            data: {
                "id": eventId,
                "__RequestVerificationToken": token
            }
        }).always(function () {
            VisibilityHelpers.loader(false);
            $("#save-entry-btn").removeAttr("disabled");
            $("#delete-entry-btn").removeAttr("disabled");
            $("#back-to-cal-btn").removeAttr("disabled");
        })
        .done(function (data: any) {
            VisibilityHelpers.alert("success", data.message, true);
            Navigate.toIndex();
        })
        .fail(function (jqXHR) {
            Site.showJqXhrAsAlert(jqXHR);
        });
    }

    /*------------------------------------------------------------------------*\
        3. Export functions
    \*------------------------------------------------------------------------*/

    /**
     * Export a calendar event to Ical.
     * @param eventId
     */
    function exportEventToIcal(eventId: string) {
        window.location.href = `/Scheduler/ExportEventToIcal/${eventId}`;
    }

    /**
     *  Export visible events on the calendar to ical.
     * */
    function exportVisibleEventsToIcal() {
        // Check if there are any events before going to the controller.
        if ($(".fc-view").has(".fc-event").length === 0) {
            VisibilityHelpers.alert("info", "There are no events to sync.", true);
            return;
        }

        const dates = SiteCalendar.getVisibleDates(calendarSelector);
        window.location.href = `/Scheduler/ExportEventsToIcal?start=${dates.start}&end=${dates.end}`;
    }

    /**
     * Export calendar events to ics from a date range.
     * @param start
     * @param end
     */
    function exportEventsFromDateRangeToIcal(start: string, end: string) {
        // Check if dates were entered before going to the server.
        if (start == null || end == null) {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: No dates provided.", true);
            return;
        }

        window.location.href = `/Scheduler/ExportEventsToIcal?start=${start}&end=${end}`;
    }

    /*------------------------------------------------------------------------*\
        4. Helper functions
    \*------------------------------------------------------------------------*/

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

                    // Stop the modal from getting 'stuck'.
                    $modal.on("hidden.bs.modal", function () {
                        Navigate.toCreateMoreOptions(title, startFormatted, endFormatted);
                        VisibilityHelpers.loader(false);
                    });
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

    /*------------------------------------------------------------------------*\
        5. Navigation functions
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
         *  Navigate to the create page.
         * */
        export function toCreate() {
            Site.loadPartial("/Scheduler/Create/");
        }

        /**
         * Navigate to the quick entry create page.
         * @param title
         * @param start
         * @param end
         */
        export function toCreateMoreOptions(title: string, start: string, end: string) {
            Site.loadPartial("/Scheduler/CreateMoreOptions/", { "title": title, "start": start, "end": end })
        }

        /**
         * Navigate to the edit event page.
         * @param id
         */
        export function toEdit(id: string) {
            Site.loadPartial(`/Scheduler/Edit/${id}`);
        }
    }
}