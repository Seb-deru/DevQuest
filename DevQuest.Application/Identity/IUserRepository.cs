using DevQuest.Domain.Identity;

namespace DevQuest.Application.Identity;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUserNameAsync(string userName);
    Task InsertAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}