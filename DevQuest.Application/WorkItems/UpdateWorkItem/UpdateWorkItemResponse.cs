namespace DevQuest.Application.WorkItems.UpdateWorkItem;

public record UpdateWorkItemResponse(Guid Id, string Name, string? Description, DateTime CreatedAt, DateTime ModifiedAt, DateTime? ScheduledDate);