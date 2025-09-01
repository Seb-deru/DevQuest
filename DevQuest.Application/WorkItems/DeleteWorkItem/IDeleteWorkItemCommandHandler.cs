namespace DevQuest.Application.WorkItems.DeleteWorkItem;

public interface IDeleteWorkItemCommandHandler
{
    Task<bool> Handle(Guid workItemId);
}