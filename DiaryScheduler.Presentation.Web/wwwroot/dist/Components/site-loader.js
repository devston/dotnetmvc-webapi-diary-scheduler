/*----------------------------------------------------------------------------*\
    
    Site loader scripts
    --------------------

    All js used for the site loader can be found here.
    
\*----------------------------------------------------------------------------*/
import $ from "jquery";
export var SiteLoader;
(function (SiteLoader) {
    /**
     * Show the loader in the given container.
     * @param selector
     */
    function show(selector) {
        if ($("#site-loader-" + selector.substr(1)).length === 0) {
            $(selector).append("<div class=\"site-loader-backdrop load-in-container\" id=\"site-loader-" + selector.substr(1) + "\">" +
                "<div class=\"site-loader-container\">" +
                "<div class=\"site-loader\"></div>" +
                "</div>" +
                "</div>").addClass("overflow-hidden position-relative");
        }
    }
    SiteLoader.show = show;
    /**
     * Remove the loader.
     * @param selector
     */
    function remove(selector) {
        if ($("#site-loader-" + selector.substr(1)).length) {
            $("#site-loader-" + selector.substr(1)).remove();
        }
        // Remove .overflow-hidden regardless as the loader maybe lost by the parents html being replaced.
        $(selector).removeClass("overflow-hidden position-relative");
    }
    SiteLoader.remove = remove;
    /**
     * Toggle visibility of the site loader.
     * @param show true = show the loader / false = hide the loader.
     */
    function toggleGlobalLoader(show) {
        if (show) {
            $("#loader-wrapper").removeClass("hidden");
        }
        else {
            $("#loader-wrapper").addClass("hidden");
        }
    }
    SiteLoader.toggleGlobalLoader = toggleGlobalLoader;
})(SiteLoader || (SiteLoader = {}));
//# sourceMappingURL=site-loader.js.map