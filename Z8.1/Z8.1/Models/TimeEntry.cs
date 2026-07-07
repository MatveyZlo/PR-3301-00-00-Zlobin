namespace TimeTracker.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public decimal Hours { get; set; }

        public string Description { get; set; } = "";

        public int WorkTaskId { get; set; }

        public WorkTask? WorkTask { get; set; }
    }
}