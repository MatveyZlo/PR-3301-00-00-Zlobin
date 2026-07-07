using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<WorkTask> Tasks => Set<WorkTask>();

        public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
    }
}