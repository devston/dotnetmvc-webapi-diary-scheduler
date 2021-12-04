/*----------------------------------------------------------------------------*\
    
    Datetime picker scripts
    -----------------------

    All js used for the site datetime picker can be found here.
    
\*----------------------------------------------------------------------------*/
import flatpickr from "flatpickr";
import "flatpickr/dist/flatpickr.css";
export var DateTimePicker;
(function (DateTimePicker) {
    /**
     * Initialise a date picker.
     * @param pickerSelector The date picker selector.
     */
    function initDate(pickerSelector) {
        flatpickr(pickerSelector, {
            altInput: true,
            altFormat: "F j, Y",
            dateFormat: "Y-m-d",
        });
    }
    DateTimePicker.initDate = initDate;
    /**
     * Initialise a datetime picker.
     * @param pickerSelector
     */
    function initDateTime(pickerSelector) {
        flatpickr(pickerSelector, {
            altInput: true,
            altFormat: "F j, Y H:i",
            enableTime: true,
            dateFormat: "Y-m-d H:i",
        });
    }
    DateTimePicker.initDateTime = initDateTime;
    /**
     * Initialise a date range between two datepickers.
     * @param startSelector
     * @param endSelector
     */
    function initDateRange(startSelector, endSelector) {
        var startDate = document.querySelector(startSelector).value;
        var endDate = document.querySelector(endSelector).value;
        var startDatepicker = flatpickr(startSelector, {
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
        var endDatepicker = flatpickr(startSelector, {
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
    DateTimePicker.initDateRange = initDateRange;
    /**
     * Initialise a datetime range between two datepickers.
     * @param startSelector
     * @param endSelector
     */
    function initDateTimeRange(startSelector, endSelector) {
        var startDate = document.querySelector(startSelector).value;
        var endDate = document.querySelector(endSelector).value;
        var startDatepicker = flatpickr(startSelector, {
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
        var endDatepicker = flatpickr(endSelector, {
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
    DateTimePicker.initDateTimeRange = initDateTimeRange;
})(DateTimePicker || (DateTimePicker = {}));
//# sourceMappingURL=site-datetimepicker.js.map