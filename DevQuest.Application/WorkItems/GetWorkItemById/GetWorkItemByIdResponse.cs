namespace DevQuest.Application.WorkItems.GetWorkItemById;

public record GetWorkItemByIdResponse(Guid Id, string Name, string? Description, DateTime CreatedAt, DateTime ModifiedAt, DateTime? ScheduledDate);