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
        SiteCalendar.init(calendarSelector, sourceUrl, showQuickCreateModal);

        $("#create-event-btn").on("click", function (e) {
            e.preventDefault();
            Navigate.toCreate();
        });
    }

    /**
     *  Initialise the create page.
     * */
    function initCreate() {
        initEventCard("#create-cal-entry-form", "/Scheduler/CreateEntry/");
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
                    $("#back-to-cal-btn").attr("disabled");
                },
                url: url,
                type: "POST",
                data: $form.serialize()
            }).always(function () {
                VisibilityHelpers.loader(false);
                $("#save-entry-btn").removeAttr("disabled");
                $("#back-to-cal-btn").removeAttr("disabled");
            })
            .done(function (data: any) {
                VisibilityHelpers.alert("success", data.message, false);
                Navigate.toIndex();
            })
            .fail(function (jqXHR) {
                Site.showJqXhrAsAlert(jqXHR);
            });
        }
    }

    /*------------------------------------------------------------------------*\
        3. Helper functions
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

                    Navigate.toCreateMoreOptions(title, startFormatted, endFormatted);

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
    }
}