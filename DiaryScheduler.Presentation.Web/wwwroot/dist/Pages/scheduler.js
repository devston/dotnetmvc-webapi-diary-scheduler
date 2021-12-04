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
export var Scheduler;
(function (Scheduler) {
    /*------------------------------------------------------------------------*\
        1. Initialisation functions
    \*------------------------------------------------------------------------*/
    var calendarSelector = "#calendar";
    var mainContainer = "#scheduler-main-container";
    /**
     * Initialise scheduler area.
     */
    function init() {
        var pageToLoad = $(mainContainer).data("page-name");
        switch (pageToLoad) {
            case "modify":
                initModify();
                break;
            default:
                initIndex();
                break;
        }
    }
    Scheduler.init = init;
    /**
     * Initialise the index page.
     */
    function initIndex() {
        var sourceUrl = document.getElementById("CalendarSourceUrl").value;
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
        var eventId = $("#CalendarEntryId").val();
        var startPickerSelector = "#DateFrom";
        var endPickerSelector = "#DateTo";
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
        var modal = bootstrap.Modal.getOrCreateInstance(document.getElementById("export-events-modal"));
        var startPickerSelector = "#SyncFrom";
        var endPickerSelector = "#SyncTo";
        var radioVal = "0";
        // Initialise the datetime pickers.
        DateTimePicker.initDateTimeRange(startPickerSelector, endPickerSelector);
        // Initialise the radio controls.
        $("#calendar-sync-options-container").find("input[name=\"calsync\"]").on("change", function () {
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
                    var start = $("#SyncFrom").val();
                    var end = $("#SyncTo").val();
                    exportEventsFromDateRangeToIcal(start, end);
                }
            }
        });
        modal.show();
    }
    /**
     *  Initialise the delete event modal.
     * */
    function initDeleteModal(eventId) {
        var modalEl = document.getElementById("confirm-delete-modal");
        var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
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
                .done(function (data) {
                SiteCalendar.addEvent(data.calEntry, calendarSelector);
                SiteAlert.show("success", data.message, true);
                var modalEl = document.getElementById("quick-create-modal");
                var modal = bootstrap.Modal.getOrCreateInstance(modalEl);
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
    function saveEvent($form) {
        var url = $(this).data("url");
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
                .done(function (data) {
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
    function deleteEvent(eventId) {
        var token = $("#edit-cal-entry-form").find("input[name=__RequestVerificationToken]").val();
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
            .done(function (data) {
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
    function exportEventToIcal(eventId) {
        window.location.href = "/Scheduler/ExportEventToIcal/" + eventId;
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
        var dates = SiteCalendar.getVisibleDates(calendarSelector);
        window.location.href = "/Scheduler/ExportEventsToIcal?start=" + dates.start + "&end=" + dates.end;
    }
    /**
     * Export calendar events to ics from a date range.
     * @param start
     * @param end
     */
    function exportEventsFromDateRangeToIcal(start, end) {
        // Check if dates were entered before going to the server.
        if (start == null || end == null) {
            SiteAlert.show("danger", "<strong>Error</strong>: No dates provided.", true);
            return;
        }
        window.location.href = "/Scheduler/ExportEventsToIcal?start=" + start + "&end=" + end;
    }
    /*------------------------------------------------------------------------*\
        4. Helper functions
    \*------------------------------------------------------------------------*/
    // Show quick create modal.
    function showQuickCreateModal(start, end) {
        SiteLoader.toggleGlobalLoader(true);
        // Format moment object to ISO 8601 so no date conversion errors are made on model bind.
        var startFormatted = start.toISOString();
        var endFormatted = end.toISOString();
        // jQuery object is used multiple times so store it in a variable.
        var $container = $("#quick-create-container");
        // Open create panel.
        $container.load("/Scheduler/_QuickCreate/", { "start": startFormatted, "end": endFormatted }, function (_response, status) {
            if (status == "success") {
                // Format date so it's human readable.
                $("#DateStarting").val(moment(start).local().format("LLL"));
                $("#DateEnding").val(moment(end).local().format("LLL"));
                var modalEl_1 = document.getElementById("quick-create-modal");
                var modal_1 = bootstrap.Modal.getOrCreateInstance(modalEl_1);
                var $form = $("#quick-create-form");
                // Show the modal.
                modal_1.show();
                // Show full create view.
                $("#edit-entry-btn").on("click", function (event) {
                    event.preventDefault();
                    SiteLoader.toggleGlobalLoader(true);
                    var title = $("#Title").val();
                    // Stop the modal from getting 'stuck'.
                    modalEl_1.addEventListener("hidden.bs.modal", function () {
                        Navigate.toCreateMoreOptions(title, startFormatted, endFormatted);
                        SiteLoader.toggleGlobalLoader(false);
                    });
                    // Close and empty the modal.
                    modal_1.hide();
                });
                // Submit form.
                $form.on("submit", function (event) {
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
    var Navigate;
    (function (Navigate) {
        /**
         * Navigate to the quick entry create page.
         * @param title
         * @param start
         * @param end
         */
        function toCreateMoreOptions(title, start, end) {
            var url = $("#CreateEventMoreOptionsUrl").val();
            url = url.replace("title_placeholder", title).replace("start_placeholder", start).replace("end_placeholder", end);
            window.location.href = url;
        }
        Navigate.toCreateMoreOptions = toCreateMoreOptions;
        /**
         * Navigate to the edit event page.
         * @param id
         */
        function toEdit(id) {
            var url = $("#EditEventUrl").val();
            url = url.replace("id_placeholder", id);
            window.location.href = url;
        }
        Navigate.toEdit = toEdit;
    })(Navigate || (Navigate = {}));
})(Scheduler || (Scheduler = {}));
// Initialise the datatables demo module on page load.
$(function () {
    Scheduler.init();
});
//# sourceMappingURL=scheduler.js.map