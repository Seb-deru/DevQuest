using DevQuest.Application.Identity;
using DevQuest.Domain.Identity;
using System.Collections.Concurrent;

namespace DevQuest.Infrastructure.Identity;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _users = new();

    public Task<User?> GetByIdAsync(Guid id)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task<User?> GetByUserNameAsync(string userName)
    {
        var user = _users.Values.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public Task InsertAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (!_users.TryAdd(user.Id, user))
            throw new InvalidOperationException($"User with ID {user.Id} already exists.");

        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _users.AddOrUpdate(user.Id, user, (key, existingUser) => user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _users.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}