using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiaryScheduler.Web.Models
{
    public class CalendarEventViewModel
    {
        public Guid CalendarEntryId { get; set; }
        public string UserId { get; set; }

        [DisplayName("From date")]
        [Required(ErrorMessage = "Select a {0}")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
               ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }

        [DisplayName("To date")]
        [Required(ErrorMessage = "Select a {0}")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
               ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; }

        [DisplayName("All day?")]
        public bool AllDay { get; set; }

        [Required(ErrorMessage = "Select a {0}")]
        [StringLength(100, ErrorMessage = "{0} must be {1} characters or less.")]
        public string Title { get; set; }

        [StringLength(200, ErrorMessage = "{0} must be {1} characters or less.")]
        public string Description { get; set; }
    }
}