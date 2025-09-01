namespace DevQuest.Application.WorkItems.UpdateWorkItem;

public record UpdateWorkItemRequest(Guid Id, string Name, string? Description = null, DateTime? ScheduledDate = null);