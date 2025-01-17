namespace DemoBackgroundService.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
