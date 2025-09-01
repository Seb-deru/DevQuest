namespace DevQuest.Application.WorkItems.GetWorkItemById;

public class GetWorkItemByIdQueryHandler : IGetWorkItemByIdQueryHandler
{
    private readonly IWorkItemRepository _workItemRepository;

    public GetWorkItemByIdQueryHandler(IWorkItemRepository workItemRepository)
    {
        _workItemRepository = workItemRepository;
    }

    public async Task<GetWorkItemByIdResponse?> Handle(Guid workItemId)
    {
        var workItem = await _workItemRepository.GetByIdAsync(workItemId);

        if (workItem == null)
            return null;

        return new GetWorkItemByIdResponse(
            workItem.Id,
            workItem.Name,
            workItem.Description,
            workItem.CreatedAt,
            workItem.ModifiedAt,
            workItem.ScheduledDate);
    }
}