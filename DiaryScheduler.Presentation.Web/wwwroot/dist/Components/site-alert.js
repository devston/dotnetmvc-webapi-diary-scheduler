/*----------------------------------------------------------------------------*\
    
    Site alert scripts
    ------------------

    All js used for the site alerts can be found here.
    
\*----------------------------------------------------------------------------*/
import $ from "jquery";
/**
 *  This module contains all the logic required for the site alert component.
 * */
export var SiteAlert;
(function (SiteAlert) {
    /**
     *  The alert container id.
     * */
    var alertContainerId = "#site-alert-container";
    /**
     * Show an alert inside the site alert container.
     * @param {string} type Alert type - "danger", "success", "info", "warning".
     * @param {string} message The alert message
     * @param {boolean} timeout Whether to auto dismiss the alert after 8 seconds (Defaults to false if not passed).
     */
    function show(type, message, timeout) {
        // Time out callback.
        $.fn.extend({
            timeout: function (ms, callback) {
                var _this = this;
                setTimeout(function () { callback.call(_this); }, ms);
                return this;
            }
        });
        // Create the alert.
        var alert = "<div class=\"alert alert-" + type + "\" role=\"alert\">\n                        " + message + "\n                        <button aria-label=\"Close\" class=\"close\" type=\"button\">\n                            <span aria-hidden=\"true\">&times;</span>\n                        </button>\n                    </div>";
        // If the alert has been set to hide, then automatically dismiss it after 8 seconds.
        if (timeout) {
            $(alert).appendTo(alertContainerId).timeout(8000, function () {
                hide($(this));
            }).children(".close").on("click", function (e) {
                e.preventDefault();
                hide($(this).parent());
            });
        }
        else {
            $(alert).appendTo(alertContainerId).children(".close").on("click", function (e) {
                e.preventDefault();
                hide($(this).parent());
            });
        }
    }
    SiteAlert.show = show;
    /**
     * Helper function to hide alerts.
     * @param {any} $alert The alert as a jQuery object.
     */
    function hide($alert) {
        $alert.remove();
    }
    SiteAlert.hide = hide;
    /**
     *  Show the jQuery XHR as a site alert.
     *  @param {any} jqXHR The jQuery XHR.
     */
    function showJqXhrError(jqXHR) {
        // Check for authentication errors (this is done using 403 instead of 401 due to asp.net pipeline).
        if (jqXHR.status === 403) {
            var response = $.parseJSON(jqXHR.responseText);
            if (response.requiresLogIn) {
                window.location.href = response.logOnUrl;
                return;
            }
        }
        // Get the error message out of the jQuery XHR.
        var errorMsg = jqXHR.statusText;
        // Show the message as an alert.
        show("danger", "<strong>Error</strong>: " + errorMsg, true);
    }
    SiteAlert.showJqXhrError = showJqXhrError;
})(SiteAlert || (SiteAlert = {}));
;
//# sourceMappingURL=site-alert.js.map