﻿namespace DiaryScheduler.Web.Models.Scheduler
{
    /// <summary>
    /// The scheduler index view model.
    /// </summary>
    public class SchedulerIndexViewModel
    {
        /// <summary>
        /// Gets or sets the create event url.
        /// </summary>
        public string CreateEventUrl { get; set; }

        /// <summary>
        /// Gets or sets the create event with more options url.
        /// </summary>
        public string CreateEventMoreOptionsUrl { get; set; }

        /// <summary>
        /// Gets or sets the edit event url.
        /// </summary>
        public string EditEventUrl { get; set; }
    }
}