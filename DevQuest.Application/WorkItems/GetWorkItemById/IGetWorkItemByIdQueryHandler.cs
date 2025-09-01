namespace DevQuest.Application.WorkItems.GetWorkItemById;

public interface IGetWorkItemByIdQueryHandler
{
    Task<GetWorkItemByIdResponse?> Handle(Guid workItemId);
}