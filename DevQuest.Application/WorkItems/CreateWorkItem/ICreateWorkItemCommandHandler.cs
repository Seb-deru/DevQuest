namespace DevQuest.Application.WorkItems.CreateWorkItem;

public interface ICreateWorkItemCommandHandler
{
    Task<CreateWorkItemResponse> Handle(CreateWorkItemRequest request);
}