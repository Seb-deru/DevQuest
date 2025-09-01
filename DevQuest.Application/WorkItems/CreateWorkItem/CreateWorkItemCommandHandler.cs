using DevQuest.Domain.WorkItems;

namespace DevQuest.Application.WorkItems.CreateWorkItem;

public class CreateWorkItemCommandHandler : ICreateWorkItemCommandHandler
{
    private readonly IWorkItemRepository _workItemRepository;

    public CreateWorkItemCommandHandler(IWorkItemRepository workItemRepository)
    {
        _workItemRepository = workItemRepository;
    }

    public async Task<CreateWorkItemResponse> Handle(CreateWorkItemRequest request)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("WorkItem name cannot be empty", nameof(request.Name));

        // Create the work item
        var workItem = new WorkItem(request.Name, request.Description, request.ScheduledDate);

        // Save to repository
        await _workItemRepository.InsertAsync(workItem);

        // Return response
        return new CreateWorkItemResponse(
            workItem.Id,
            workItem.Name,
            workItem.Description,
            workItem.CreatedAt,
            workItem.ModifiedAt,
            workItem.ScheduledDate);
    }
}