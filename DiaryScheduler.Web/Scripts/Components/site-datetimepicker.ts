/*----------------------------------------------------------------------------*\
    
    Datetime picker scripts
    -----------------------

    All js used for the site datetime picker can be found here.
    
\*----------------------------------------------------------------------------*/

import flatpickr from "flatpickr";
import rangePlugin from "flatpickr/dist/plugins/rangePlugin";

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
        flatpickr(startSelector, {
            altInput: true,
            altFormat: "F j, Y",
            enableTime: true,
            dateFormat: "Y-m-d",
            // @ts-ignore: Included types have not configured the range plugin.
            plugins: [new rangePlugin({ input: endSelector })]
        });
    }

    /**
     * Initialise a datetime range between two datepickers.
     * @param startSelector
     * @param endSelector
     */
    export function initDateTimeRange(startSelector: string, endSelector: string) {
        flatpickr(startSelector, {
            altInput: true,
            altFormat: "F j, Y H:i",
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            // @ts-ignore: Included types have not configured the range plugin.
            plugins: [new rangePlugin({ input: endSelector })]
        });
    }
}