namespace DevQuest.Application.WorkItems.UpdateWorkItem;

public interface IUpdateWorkItemCommandHandler
{
    Task<UpdateWorkItemResponse?> Handle(UpdateWorkItemRequest request);
}