/*----------------------------------------------------------------------------*\
    
    Site loader scripts
    --------------------

    All js used for the site loader can be found here.
    
\*----------------------------------------------------------------------------*/

import $ from "jquery";

export namespace SiteLoader {

    /**
     * Show the loader in the given container.
     * @param selector
     */
    export function show(selector: string) {
        if ($("#site-loader-" + selector.substr(1)).length === 0) {
            $(selector).append("<div class=\"site-loader-backdrop load-in-container\" id=\"site-loader-" + selector.substr(1) + "\">" +
                "<div class=\"site-loader-container\">" +
                "<div class=\"site-loader\"></div>" +
                "</div>" +
                "</div>").addClass("overflow-hidden position-relative");
        }
    }

    /**
     * Remove the loader.
     * @param selector
     */
    export function remove(selector: string) {
        if ($("#site-loader-" + selector.substr(1)).length) {
            $("#site-loader-" + selector.substr(1)).remove();
        }

        // Remove .overflow-hidden regardless as the loader maybe lost by the parents html being replaced.
        $(selector).removeClass("overflow-hidden position-relative");
    }
}