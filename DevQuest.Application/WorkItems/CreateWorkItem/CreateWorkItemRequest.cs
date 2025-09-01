namespace DevQuest.Application.WorkItems.CreateWorkItem;

public record CreateWorkItemRequest(string Name, string? Description = null, DateTime? ScheduledDate = null);