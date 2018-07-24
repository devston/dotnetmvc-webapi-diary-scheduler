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
import "fullcalendar";
//import { Site } from "Scripts/Utilities/site-core";

/**
 * A module containing all the logic for the scheduler area.
 */
export namespace Scheduler {

    /*------------------------------------------------------------------------*\
        1. Initialisation functions
    \*------------------------------------------------------------------------*/

    /**
     * Initialise scheduler area.
     */
    export function init(pageToLoad: string) {
        switch (pageToLoad) {
            default:
                initIndex();
                break;
        }
    }

    /**
     * Initialise the index page.
     */
    function initIndex() {
        $("#calendar").fullCalendar();
    }

    /*------------------------------------------------------------------------*\
        2. Create, update and delete functions
    \*------------------------------------------------------------------------*/



    /*------------------------------------------------------------------------*\
        3. Helper functions
    \*------------------------------------------------------------------------*/



    /*------------------------------------------------------------------------*\
        4. Navigation functions
    \*------------------------------------------------------------------------*/

    /**
     * Contains navigation based functionality for the scheduler area.
     */
    //namespace Navigate {
    //    /**
    //     * Navigate to the scheduler index page.
    //     */
    //    export function toIndex() {
    //        Site.loadPartial("/Scheduler/");
    //    }
    //}
}