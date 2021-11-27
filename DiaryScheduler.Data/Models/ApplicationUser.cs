using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DiaryScheduler.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<CalendarEvent> CalendarEntries { get; set; }
    }
}
