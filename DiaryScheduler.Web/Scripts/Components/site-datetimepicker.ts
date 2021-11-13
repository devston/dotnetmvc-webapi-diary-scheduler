/*----------------------------------------------------------------------------*\
    
    Datetime picker scripts
    -----------------------

    All js used for the site datetime picker can be found here.
    
\*----------------------------------------------------------------------------*/

import flatpickr from "flatpickr";
import "flatpickr/dist/flatpickr.css";

export namespace DateTimePicker {
    /**
     * Initialise a date picker.
     * @param pickerSelector The date picker selector.
     */
    export function initDate(pickerSelector: string) {
        flatpickr(pickerSelector, {
            altInput: true,
            altFormat: "F j, Y",
            dateFormat: "Y-m-d",
        });
    }

    /**
     * Initialise a datetime picker.
     * @param pickerSelector
     */
    export function initDateTime(pickerSelector: string) {
        flatpickr(pickerSelector, {
            altInput: true,
            altFormat: "F j, Y H:i",
            enableTime: true,
            dateFormat: "Y-m-d H:i",
        });
    }

    /**
     * Initialise a date range between two datepickers.
     * @param startSelector
     * @param endSelector
     */
    export function initDateRange(startSelector: string, endSelector: string) {
        const startDate = (document.querySelector(startSelector) as HTMLInputElement).value;
        const endDate = (document.querySelector(endSelector) as HTMLInputElement).value;
        const startDatepicker = flatpickr(startSelector, {
            altInput: true,
            altFormat: "F j, Y",
            enableTime: true,
            dateFormat: "Y-m-d",
            maxDate: endDate,
            onChange: function (selectedDates, dateStr, instance) {
                // @ts-ignore: Flatpickr types not being picked up correctly.
                endDatepicker.set("minDate", selectedDates[0]);
            }
        });

        const endDatepicker = flatpickr(startSelector, {
            altInput: true,
            altFormat: "F j, Y",
            enableTime: true,
            dateFormat: "Y-m-d",
            minDate: startDate,
            onChange: function (selectedDates, dateStr, instance) {
                // @ts-ignore: Flatpickr types not being picked up correctly.
                startDatepicker.set("maxDate", selectedDates[0]);
            }
        });
    }

    /**
     * Initialise a datetime range between two datepickers.
     * @param startSelector
     * @param endSelector
     */
    export function initDateTimeRange(startSelector: string, endSelector: string) {
        const startDate = (document.querySelector(startSelector) as HTMLInputElement).value;
        const endDate = (document.querySelector(endSelector) as HTMLInputElement).value;
        const startDatepicker = flatpickr(startSelector, {
            altInput: true,
            altFormat: "F j, Y H:i",
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            maxDate: endDate,
            onChange: function (selectedDates, dateStr, instance) {
                // @ts-ignore: Flatpickr types not being picked up correctly.
                endDatepicker.set("minDate", selectedDates[0]);
            }
        });

        const endDatepicker = flatpickr(endSelector, {
            altInput: true,
            altFormat: "F j, Y H:i",
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            minDate: startDate,
            onChange: function (selectedDates, dateStr, instance) {
                // @ts-ignore: Flatpickr types not being picked up correctly.
                startDatepicker.set("maxDate", selectedDates[0]);
            }
        });
    }
}