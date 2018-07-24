import $ from "jquery";
import "jquery-pjax";
import { VisibilityHelpers } from "Scripts/Utilities/visibility-helpers";

/**
 * A module containing all of our core site functionality.
 */
export namespace Site {
    const mainContainerId = "pjax-container";

    /**
     * Initialise the site.
     */
    export function init() {
        if ($.support.pjax) {
            $.pjax.defaults.timeout = 5000;
            $.pjax.defaults.maxCacheLength = 0;

            $(document).on("pjax:beforeSend", function () {

            });

            $(document).on("pjax:send", function () {
                VisibilityHelpers.loader(true);
            });

            $(document).on("pjax:timeout", function (event) {
                // Prevent default timeout redirection behaviour
                event.preventDefault();
                var loadDelayHtml = require("handlebarsTemplates/load-delay")({});
                $(loadDelayHtml).appendTo("#alert-container");
                $("#alert-container").find(".js-hard-refresh").on("click", function (e) {
                    e.preventDefault();
                    location.reload(true);
                });
            });

            // Place _ infront of the variables to keep typescripts unused variable error handling from firing.
            $(document).on("pjax:error", function (_xhr, _textStatus, error, _options) {
                VisibilityHelpers.alert("danger", `<strong>Error</strong>: ${error}`, false);
            });

            $(document).on("pjax:success", function (data) {
                callPageInit(data);
            });

            $(document).on("pjax:complete", function (event) {
                $(event.target).removeClass("hidden");
                $("#delay-alert-container").remove();
                VisibilityHelpers.loader(false);
            });

            $(document).on("pjax:end", function () {

            });

            $(document).on("pjax:popstate", function () {
                // Reload the page if the popstate is fired.
                // This seemed like a necessary evil as pjax caches a lot of the page,
                // so incorrect data was being shown, duplicate controls (if controls were initialised)
                // whereas reloading the page alleviated these problems.
                location.reload(true);
            });

            // Load any embedded modules on first load.
            callPageInit({ "target": { "id": mainContainerId } });

            // Only use pjax for the nav links container, so log in actions do full postbacks.
            const $navLinks = $("#main-nav-links");

            // Use pjax for the nav.
            $navLinks.on("click", "a", function (e) {
                e.preventDefault();
                const $this = $(this);
                const href = <string>$this.attr("href");

                // Check for empty hrefs.
                if (!href || href === "#") {
                    return;
                }

                switchActiveLink($this);
                loadPartial(href);
            });
        }
        else {
            VisibilityHelpers.alert("danger", "<strong>Error</strong>: Pjax failed to load.", false);
        }
    }

    /**
     * Load a partial through pjax.
     * @param url The url to fetch the partial from.
     * @param containerSelector The container the partial should be loaded in (if not provided then this will default to the pjax container).
     * @param callback A callback that should be called after the partial has loaded.
     * @param data Any data to pass the partial.
     * @param requestType The request type.
     */
    export function loadPartial(url: string, data?: any, containerSelector?: string, callback?: any, requestType?: string) {
        // Default the container id to the main container id if nothing has been provided.
        if (!containerSelector || containerSelector.length === 0) {
            containerSelector = mainContainerId;
        }

        // Check if the container id begins with an # or .
        if (containerSelector.charAt(0) != "#" || containerSelector.charAt(0) != ".") {
            containerSelector = `#${containerSelector}`;
        }

        // Check if a request type has been provided.
        if (!requestType || requestType.length === 0) {
            requestType = "GET";
        }

        $.pjax({
            "type": requestType,
            "url": url,
            "container": containerSelector,
            "data": data
        })
        .done(callback);
    }

    /**
     * Show the jQuery XHR as a site alert.
     * @param jqXhr
     */
    export function showJqXhrAsAlert(jqXHR: JQuery.jqXHR) {
        var errorMsg = jqXHR.responseText;

        if (jqXHR.responseJSON) {
            errorMsg = jqXHR.responseJSON[0].ErrorMessage;
        }

        // Check if an asp.net error page was returned and take the error from the page.
        if (errorMsg.indexOf("DOCTYPE") !== -1) {
            errorMsg = $(errorMsg).find("h2").text();
        }

        VisibilityHelpers.alert("danger", `<strong>Error</strong>: ${errorMsg}`, true);
    }

    /**
     * Call the page initialise.
     * @param data
     */
    function callPageInit(data: any) {
        let containerId = data.target.id;

        if (containerId === mainContainerId) {
            const hasRequire = $(`#${containerId}`).find("[page-require]");

            if (hasRequire.length > 0) {
                const ctrl = hasRequire.attr("page-require");
                const moduleName = hasRequire.attr("page-module-name");
                let moduleVariable = hasRequire.attr("page-module-variable");
                let moduleObject = require(`Scripts/Pages/${ctrl}`);

                if (moduleName == undefined) {
                    // Check if there is a module with the require name,
                    // if present use it. Otherwise this has a default module
                    // which is already available in the module object.
                    if (ctrl != undefined && moduleObject[ctrl] != undefined) {
                        moduleObject = moduleObject[ctrl];
                    }
                }

                // Module name has been specified.
                else {
                    // Use the provided module as the module object.
                    moduleObject = moduleObject[moduleName];
                }

                // Check if there was a module variable.
                if (moduleVariable == undefined) {
                    // Default the module variable to index if there was not one.
                    moduleVariable = "index";
                }

                if (moduleObject.init != undefined) {
                    moduleObject.init(moduleVariable);
                }
                else {
                    // Check if it's a class type declaration by creating it as an object.
                    const mo = new moduleObject();

                    if (mo.init != undefined) {
                        mo.init(moduleVariable);
                    }
                    else {
                        // Exhausted all available options, so throw an error.
                        VisibilityHelpers.alert("danger", `Cannot find init method for ${ctrl}`);
                    }
                }
            }
        }
    }

    /**
     * Switch the active navigation link.
     * @param $clickedLink
     */
    function switchActiveLink($clickedLink: JQuery<HTMLElement>) {
        // Get the parent li, which will have the .active class applied.
        const $parentLi = $clickedLink.parent();

        // Find the li that currently has the .active class applied and remove it.
        const $currentActive = $parentLi.siblings(".active");
        $currentActive.removeClass("active");

        // Apply the .active class to the clicked link.
        $parentLi.addClass("active");
    }
};