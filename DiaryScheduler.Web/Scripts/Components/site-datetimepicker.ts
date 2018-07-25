/*----------------------------------------------------------------------------*\
    
    Datetime picker scripts
    -----------------------

    All js used for the site datetime picker can be found here.
    
\*----------------------------------------------------------------------------*/

import $ from "jquery";
import moment from "moment";
import "eonasdan-bootstrap-datetimepicker-bootstrap4beta";
import "eonasdan-bootstrap-datetimepicker-bootstrap4beta/build/css/bootstrap-datetimepicker.min.css";

export namespace DateTimePicker {
    /**
     * Initialise a date picker.
     * @param pickerSelector The date picker selector.
     */
    export function initDate(pickerSelector: string) {
        // jQuery objects are used multiple times so store them in variables.
        var $picker = $(pickerSelector);
        var valueSelector = "#" + $picker.attr("data-id");
        var $value = $(valueSelector);

        // Convert the time from utc to the user's timezone.
        var date = moment($value.val(), "YYYY-MM-DD").local();

        $picker.datetimepicker({
            format: "LL",
            icons: {
                time: "far fa-clock",
                date: "far fa-calendar-alt",
                up: "fas fa-angle-up",
                down: "fas fa-angle-down",
                previous: "fas fa-angle-left",
                next: "fas fa-angle-right",
                today: "far fa-calendar-check",
                clear: "fas fa-trash-alt",
                close: "fas fa-times"
            },
        })
        .data("DateTimePicker").date(date);

        $picker.on("dp.change", function (e) {
            $value.val(moment(e.date).format("YYYY-MM-DD"));
        });
    }

    /**
     * Initialise a datetime picker.
     * @param pickerSelector
     */
    export function initDateTime(pickerSelector: string) {
        // jQuery objects are used multiple times so store them in variables.
        var $picker = $(pickerSelector);
        var valueSelector = "#" + $picker.attr("data-id");
        var $value = $(valueSelector);

        // Convert the time from utc to the user's timezone.
        var date = moment($value.val(), "YYYY-MM-DD HH:mm").local();

        $picker.datetimepicker({
            format: "llll",
            icons: {
                time: "far fa-clock",
                date: "far fa-calendar-alt",
                up: "fas fa-angle-up",
                down: "fas fa-angle-down",
                previous: "fas fa-angle-left",
                next: "fas fa-angle-right",
                today: "far fa-calendar-check",
                clear: "fas fa-trash-alt",
                close: "fas fa-times"
            },
        })
        .data("DateTimePicker").date(date);

        $picker.on("dp.change", function (e) {
            $value.val(moment(e.date).format("YYYY-MM-DD HH:mm"));
        });
    }

    /**
     * Initialise a date range between two datepickers.
     * @param startSelector
     * @param endSelector
     */
    export function initRange(startSelector: string, endSelector: string) {
        const $startPicker = $(startSelector);
        const $endPicker = $(endSelector);

        $startPicker.on("dp.change", function (event) {
            // Date range.
            if (moment($endPicker.data("DateTimePicker").date()).isBefore(event.date)) {
                $endPicker.data("DateTimePicker").date(moment(event.date));
            }
        });

        $endPicker.on("dp.change", function (event) {
            if (moment($startPicker.data("DateTimePicker").date()).isAfter(event.date)) {
                $startPicker.data("DateTimePicker").date(moment(event.date));
            }
        });
    }
}