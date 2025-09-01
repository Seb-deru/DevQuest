namespace DevQuest.Application.WorkItems.GetAllWorkItems;

public class GetAllWorkItemsQueryHandler : IGetAllWorkItemsQueryHandler
{
    private readonly IWorkItemRepository _workItemRepository;

    public GetAllWorkItemsQueryHandler(IWorkItemRepository workItemRepository)
    {
        _workItemRepository = workItemRepository;
    }

    public async Task<GetAllWorkItemsResponse> Handle()
    {
        var workItems = await _workItemRepository.GetAllAsync();

        var workItemSummaries = workItems.Select(workItem => new WorkItemSummary(
            workItem.Id,
            workItem.Name,
            workItem.Description,
            workItem.CreatedAt,
            workItem.ModifiedAt,
            workItem.ScheduledDate));

        return new GetAllWorkItemsResponse(workItemSummaries);
    }
}