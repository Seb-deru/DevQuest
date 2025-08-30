using DevQuest.Domain.Identity;

namespace DevQuest.Application.Identity.CreateUser;

public class CreateUserCommandHandler : ICreateUserCommandHandler
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(request.UserName))
            throw new ArgumentException("Username cannot be empty", nameof(request.UserName));

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email cannot be empty", nameof(request.Email));

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password cannot be empty", nameof(request.Password));

        // Check if user already exists by email or username
        var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUserByEmail != null)
            throw new InvalidOperationException($"User with email '{request.Email}' already exists");

        var existingUserByUsername = await _userRepository.GetByUserNameAsync(request.UserName);
        if (existingUserByUsername != null)
            throw new InvalidOperationException($"User with username '{request.UserName}' already exists");

        // Hash the password (for now, we'll use a simple hash - in production, use proper password hashing)
        var passwordHash = HashPassword(request.Password);

        // Create the user
        var user = new User(request.UserName, request.Email, passwordHash);

        // Save to repository
        await _userRepository.InsertAsync(user);

        // Return response
        return new CreateUserResponse(user.Id, user.UserName, user.Email);
    }

    private static string HashPassword(string password)
    {
        // Simple hash for demonstration - in production, use BCrypt, Argon2, or similar
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"hashed_{password}"));
    }
}