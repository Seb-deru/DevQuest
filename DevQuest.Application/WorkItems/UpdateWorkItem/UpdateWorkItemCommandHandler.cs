namespace DevQuest.Application.WorkItems.UpdateWorkItem;

public class UpdateWorkItemCommandHandler : IUpdateWorkItemCommandHandler
{
    private readonly IWorkItemRepository _workItemRepository;

    public UpdateWorkItemCommandHandler(IWorkItemRepository workItemRepository)
    {
        _workItemRepository = workItemRepository;
    }

    public async Task<UpdateWorkItemResponse?> Handle(UpdateWorkItemRequest request)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("WorkItem name cannot be empty", nameof(request.Name));

        // Get existing work item
        var existingWorkItem = await _workItemRepository.GetByIdAsync(request.Id);
        if (existingWorkItem == null)
            return null;

        // Update the work item
        existingWorkItem.Update(request.Name, request.Description, request.ScheduledDate);

        // Save to repository
        await _workItemRepository.UpdateAsync(existingWorkItem);

        // Return response
        return new UpdateWorkItemResponse(
            existingWorkItem.Id,
            existingWorkItem.Name,
            existingWorkItem.Description,
            existingWorkItem.CreatedAt,
            existingWorkItem.ModifiedAt,
            existingWorkItem.ScheduledDate);
    }
}