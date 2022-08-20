using DiaryScheduler.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DiaryScheduler.Data.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CalendarEvent> CalendarEvents { get; set; }
}
