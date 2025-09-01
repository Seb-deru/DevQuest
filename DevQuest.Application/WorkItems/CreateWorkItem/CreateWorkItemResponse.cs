namespace DevQuest.Application.WorkItems.CreateWorkItem;

public record CreateWorkItemResponse(Guid Id, string Name, string? Description, DateTime CreatedAt, DateTime ModifiedAt, DateTime? ScheduledDate);