using DevQuest.Domain.WorkItems;

namespace DevQuest.Application.WorkItems;

public interface IWorkItemRepository
{
    Task<WorkItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<WorkItem>> GetAllAsync();
    Task<IEnumerable<WorkItem>> GetByNameAsync(string name);
    Task<IEnumerable<WorkItem>> GetScheduledForDateAsync(DateTime date);
    Task InsertAsync(WorkItem workItem);
    Task UpdateAsync(WorkItem workItem);
    Task DeleteAsync(Guid id);
}