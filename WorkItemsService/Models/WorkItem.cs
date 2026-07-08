namespace WorkItemsService.Models
{
    public class WorkItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string? AssignedUsername { get; set; }
    }
}