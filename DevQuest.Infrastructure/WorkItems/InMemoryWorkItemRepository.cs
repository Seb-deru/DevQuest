using DevQuest.Application.WorkItems;
using DevQuest.Domain.WorkItems;
using System.Collections.Concurrent;

namespace DevQuest.Infrastructure.WorkItems;

public class InMemoryWorkItemRepository : IWorkItemRepository
{
    private readonly ConcurrentDictionary<Guid, WorkItem> _workItems = new();

    public Task<WorkItem?> GetByIdAsync(Guid id)
    {
        _workItems.TryGetValue(id, out var workItem);
        return Task.FromResult(workItem);
    }

    public Task<IEnumerable<WorkItem>> GetAllAsync()
    {
        var workItems = _workItems.Values.AsEnumerable();
        return Task.FromResult(workItems);
    }

    public Task<IEnumerable<WorkItem>> GetByNameAsync(string name)
    {
        var workItems = _workItems.Values
            .Where(w => w.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .AsEnumerable();
        return Task.FromResult(workItems);
    }

    public Task<IEnumerable<WorkItem>> GetScheduledForDateAsync(DateTime date)
    {
        var workItems = _workItems.Values
            .Where(w => w.ScheduledDate.HasValue && w.ScheduledDate.Value.Date == date.Date)
            .AsEnumerable();
        return Task.FromResult(workItems);
    }

    public Task InsertAsync(WorkItem workItem)
    {
        if (workItem == null)
            throw new ArgumentNullException(nameof(workItem));

        if (!_workItems.TryAdd(workItem.Id, workItem))
            throw new InvalidOperationException($"WorkItem with ID {workItem.Id} already exists.");

        return Task.CompletedTask;
    }

    public Task UpdateAsync(WorkItem workItem)
    {
        if (workItem == null)
            throw new ArgumentNullException(nameof(workItem));

        _workItems.AddOrUpdate(workItem.Id, workItem, (key, existingWorkItem) => workItem);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _workItems.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}