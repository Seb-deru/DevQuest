namespace DevQuest.Application.WorkItems.GetAllWorkItems;

public record WorkItemSummary(Guid Id, string Name, string? Description, DateTime CreatedAt, DateTime ModifiedAt, DateTime? ScheduledDate);

public record GetAllWorkItemsResponse(IEnumerable<WorkItemSummary> WorkItems);