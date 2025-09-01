namespace DevQuest.Domain.WorkItems;

public class WorkItem
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }
    public DateTime? ScheduledDate { get; private set; }

    public WorkItem(string name, string? description = null, DateTime? scheduledDate = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("WorkItem name cannot be null, empty, or whitespace.", nameof(name));

        Name = name;
        Description = description;
        ScheduledDate = scheduledDate;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("WorkItem name cannot be null, empty, or whitespace.", nameof(name));

        Name = name;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateScheduledDate(DateTime? scheduledDate)
    {
        ScheduledDate = scheduledDate;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description = null, DateTime? scheduledDate = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("WorkItem name cannot be null, empty, or whitespace.", nameof(name));

        Name = name;
        Description = description;
        ScheduledDate = scheduledDate;
        ModifiedAt = DateTime.UtcNow;
    }
}