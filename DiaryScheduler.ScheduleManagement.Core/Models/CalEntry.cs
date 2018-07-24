using System;
using System.ComponentModel.DataAnnotations;

namespace DiaryScheduler.ScheduleManagement.Core.Models
{
    public class CalEntry
    {
        public Guid CalendarEntryId { get; set; }
        public string UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool AllDay { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
