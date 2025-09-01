namespace DevQuest.Application.WorkItems.GetAllWorkItems;

public interface IGetAllWorkItemsQueryHandler
{
    Task<GetAllWorkItemsResponse> Handle();
}