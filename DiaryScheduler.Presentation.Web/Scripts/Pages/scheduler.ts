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

import * as bootstrap from "bootstrap";
import $ from "jquery";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import moment from "moment";
import { SiteCalendar } from "./../Components/site-calendar";
import { DateTimePicker } from "./../Components/site-datetimepicker";
import { SiteAlert } from "./../Components/site-alert";
import { SiteLoader } from "./../Components/site-loader";

/**
 * A module containing all the logic for the scheduler area.
 */
export namespace Scheduler {

    /*------------------------------------------------------------------------*\
        1. Initialisation functions
    \*------------------------------------------------------------------------*/

    const calendarSelector = "#calendar";
    const mainContainer = "#scheduler-main-container";

    /**
     * Initialise scheduler area.
     */
    export function init() {
        const pageToLoad = $(mainContainer).data("page-name");

        switch (pageToLoad) {
            case "modify":
                initModify();
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
        const sourceUrl = (document.getElementById("CalendarSourceUrl") as HTMLInputElement).value;
        SiteCalendar.init(calendarSelector, sourceUrl, showQuickCreateModal, Navigate.toEdit);

        $("#export-events-btn").on("click", function (e) {
            e.preventDefault();
            initExportModal();
        });
    }

    /**
     *  Initialise the edit event page.
     * */
    function initModify() {
        const eventId = <string>$("#CalendarEntryId").val();
        const startPickerSelector = "#DateFrom";
        const endPickerSelector = "#DateTo";

        // Initialise the datetime pickers.
        DateTimePicker.initDateTimeRange(startPickerSelector, endPickerSelector);

        // Form submit.
        $("#edit-cal-entry-form").on("submit", function (e) {
            e.preventDefault();
            saveEvent($(this));
        });

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
     *  Initialise the export calendar event modal.
     * */
    function initExportModal() {
        const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById("export-events-modal"));
        const startPickerSelector = "#SyncFrom";
        const endPickerSelector = "#SyncTo";
        let radioVal = "0";

        // Initialise the datetime pickers.
        DateTimePicker.initDateTimeRange(startPickerSelector, endPickerSelector);

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
                modal.hide();

                if (radioVal == "1") {
                    exportVisibleEventsToIcal();
                }
                else if (radioVal == "2") {
                    var start = <string>$("#SyncFrom").val();
                    var end = <string>$("#SyncTo").val();

                    exportEventsFromDateRangeToIcal(start, end);
                }
            }
        });

        modal.show();
    }

    /**
     *  Initialise the delete event modal.
     * */
    function initDeleteModal(eventId: string) {
        const modalEl = document.getElementById("confirm-delete-modal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);

        $("#confirm-delete-btn").on("click", function (e) {
            e.preventDefault();

            // Stop the modal from getting 'stuck'.
            modalEl.addEventListener("hidden.bs.modal", function () {
                deleteEvent(eventId);
            });

            modal.hide();
        });

        modal.show();
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
                    SiteLoader.toggleGlobalLoader(true);
                    $("#edit-entry-btn").attr("disabled");
                    $("#quick-create-btn").attr("disabled");
                },
                url: "/Scheduler/CreateEntry/",
                type: "POST",
                data: $form.serialize()
            })
                .always(function () {
                    SiteLoader.toggleGlobalLoader(false);
                    $("#edit-entry-btn").removeAttr("disabled");
                    $("#quick-create-btn").removeAttr("disabled");
                })
                .done(function (data: any) {
                    SiteCalendar.addEvent(data.calEntry, calendarSelector);
                    SiteAlert.show("success", data.message, true);
                    const modalEl = document.getElementById("quick-create-modal");
                    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);

                    modalEl.addEventListener("hidden.bs.modal", function () {
                        $("#quick-create-container").empty();
                    });

                    // Close and empty the modal.
                    modal.hide();
                })
                .fail(function (jqXHR) {
                    SiteAlert.showJqXhrError(jqXHR);
                });
        }
    }

    /**
     * Save a calendar event.
     * @param formId
     * @param url
     */
    function saveEvent($form: JQuery<HTMLElement>) {
        const url = $(this).data("url");

        if ($form.valid()) {
            $.ajax({
                beforeSend: function () {
                    SiteLoader.toggleGlobalLoader(true);
                    $("#save-entry-btn").attr("disabled");
                    $("#delete-entry-btn").attr("disabled");
                    $("#back-to-cal-btn").attr("disabled");
                },
                url: url,
                type: "POST",
                data: $form.serialize()
            }).always(function () {
                SiteLoader.toggleGlobalLoader(false);
                $("#save-entry-btn").removeAttr("disabled");
                $("#delete-entry-btn").removeAttr("disabled");
                $("#back-to-cal-btn").removeAttr("disabled");
            })
                .done(function (data: any) {
                    SiteAlert.show("success", data.message, true);
                    window.location.href = data.backUrl;
                })
                .fail(function (jqXHR) {
                    SiteAlert.showJqXhrError(jqXHR);
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
                SiteLoader.toggleGlobalLoader(true);
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
            SiteLoader.toggleGlobalLoader(false);
            $("#save-entry-btn").removeAttr("disabled");
            $("#delete-entry-btn").removeAttr("disabled");
            $("#back-to-cal-btn").removeAttr("disabled");
        })
            .done(function (data: any) {
                SiteAlert.show("success", data.message, true);
                window.location.href = data.backUrl;
            })
            .fail(function (jqXHR) {
                SiteAlert.showJqXhrError(jqXHR);
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
            SiteAlert.show("info", "There are no events to sync.", true);
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
            SiteAlert.show("danger", "<strong>Error</strong>: No dates provided.", true);
            return;
        }

        window.location.href = `/Scheduler/ExportEventsToIcal?start=${start}&end=${end}`;
    }

    /*------------------------------------------------------------------------*\
        4. Helper functions
    \*------------------------------------------------------------------------*/

    // Show quick create modal.
    function showQuickCreateModal(start: Date, end: Date) {
        SiteLoader.toggleGlobalLoader(true);

        // Format moment object to ISO 8601 so no date conversion errors are made on model bind.
        const startFormatted = start.toISOString();
        const endFormatted = end.toISOString();

        // jQuery object is used multiple times so store it in a variable.
        var $container = $("#quick-create-container");

        // Open create panel.
        $container.load("/Scheduler/_QuickCreate/", { "start": startFormatted, "end": endFormatted }, function (_response: string, status: string) {
            if (status == "success") {
                // Format date so it's human readable.
                $("#DateStarting").val(moment(start).local().format("LLL"));
                $("#DateEnding").val(moment(end).local().format("LLL"));
                const modalEl = document.getElementById("quick-create-modal");
                const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
                var $form = $("#quick-create-form");

                // Show the modal.
                modal.show();

                // Show full create view.
                $("#edit-entry-btn").on("click", function (event: JQueryEventObject) {
                    event.preventDefault();

                    SiteLoader.toggleGlobalLoader(true);
                    var title = <string>$("#Title").val();

                    // Stop the modal from getting 'stuck'.
                    modalEl.addEventListener("hidden.bs.modal", function () {
                        Navigate.toCreateMoreOptions(title, startFormatted, endFormatted);
                        SiteLoader.toggleGlobalLoader(false);
                    });

                    // Close and empty the modal.
                    modal.hide();
                });

                // Submit form.
                $form.on("submit", function (event: JQueryEventObject) {
                    event.preventDefault();
                    quickCreate();
                });

                SiteLoader.toggleGlobalLoader(false);
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
         * Navigate to the quick entry create page.
         * @param title
         * @param start
         * @param end
         */
        export function toCreateMoreOptions(title: string, start: string, end: string) {
            let url = $("#CreateEventMoreOptionsUrl").val() as string;
            url = url.replace("title_placeholder", title).replace("start_placeholder", start).replace("end_placeholder", end);
            window.location.href = url;
        }

        /**
         * Navigate to the edit event page.
         * @param id
         */
        export function toEdit(id: string) {
            let url = $("#EditEventUrl").val() as string;
            url = url.replace("id_placeholder", id);
            window.location.href = url;
        }
    }
}

// Initialise the datatables demo module on page load.
$(() => {
    Scheduler.init();
});