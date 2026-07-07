namespace TimeTracker.Models
{
    public class WorkTask
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public bool IsActive { get; set; }

        public int ProjectId { get; set; }

        public Project? Project { get; set; }

        public ICollection<TimeEntry>? TimeEntries { get; set; }
    }
}