namespace DevQuest.Domain.Identity;
public class User
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;


    public User(string userName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Username cannot be null, empty, or whitespace.", nameof(userName));

        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
    }
}
