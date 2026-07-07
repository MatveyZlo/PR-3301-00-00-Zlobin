namespace TimeTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Code { get; set; } = "";

        public bool IsActive { get; set; }

        public ICollection<WorkTask>? Tasks { get; set; }
    }
}