namespace DevQuest.Application.WorkItems.DeleteWorkItem;

public class DeleteWorkItemCommandHandler : IDeleteWorkItemCommandHandler
{
    private readonly IWorkItemRepository _workItemRepository;

    public DeleteWorkItemCommandHandler(IWorkItemRepository workItemRepository)
    {
        _workItemRepository = workItemRepository;
    }

    public async Task<bool> Handle(Guid workItemId)
    {
        // Check if work item exists
        var existingWorkItem = await _workItemRepository.GetByIdAsync(workItemId);
        if (existingWorkItem == null)
            return false;

        // Delete the work item
        await _workItemRepository.DeleteAsync(workItemId);
        return true;
    }
}