using DevQuest.Domain.Identity;
using DevQuest.Infrastructure.Identity;

namespace DevQuest.Infrastructure.Tests.Identity;

public class InMemoryUserRepositoryTests
{
    private readonly InMemoryUserRepository _repository;

    public InMemoryUserRepositoryTests()
    {
        _repository = new InMemoryUserRepository();
    }

    [Fact]
    public async Task InsertAsync_Should_Add_User_Successfully()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");

        // Act
        await _repository.InsertAsync(user);

        // Assert
        var retrievedUser = await _repository.GetByIdAsync(user.Id);
        retrievedUser.Should().NotBeNull();
        retrievedUser!.Id.Should().Be(user.Id);
        retrievedUser.UserName.Should().Be(user.UserName);
        retrievedUser.Email.Should().Be(user.Email);
        retrievedUser.PasswordHash.Should().Be(user.PasswordHash);
    }

    [Fact]
    public async Task InsertAsync_Should_Throw_ArgumentNullException_When_User_Is_Null()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.InsertAsync(null!));
        exception.ParamName.Should().Be("user");
    }

    [Fact]
    public async Task InsertAsync_Should_Throw_InvalidOperationException_When_User_Already_Exists()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.InsertAsync(user));
        exception.Message.Should().Contain($"User with ID {user.Id} already exists");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_User_When_User_Exists()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Should().Be(user);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_User_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_Should_Return_User_When_User_Exists()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);

        // Act
        var result = await _repository.GetByEmailAsync("test@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.Should().Be(user);
    }

    [Fact]
    public async Task GetByEmailAsync_Should_Be_Case_Insensitive()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);

        // Act
        var result = await _repository.GetByEmailAsync("TEST@EXAMPLE.COM");

        // Assert
        result.Should().NotBeNull();
        result!.Should().Be(user);
    }

    [Fact]
    public async Task GetByEmailAsync_Should_Return_Null_When_User_Does_Not_Exist()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUserNameAsync_Should_Return_User_When_User_Exists()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);

        // Act
        var result = await _repository.GetByUserNameAsync("testuser");

        // Assert
        result.Should().NotBeNull();
        result!.Should().Be(user);
    }

    [Fact]
    public async Task GetByUserNameAsync_Should_Be_Case_Insensitive()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);

        // Act
        var result = await _repository.GetByUserNameAsync("TESTUSER");

        // Assert
        result.Should().NotBeNull();
        result!.Should().Be(user);
    }

    [Fact]
    public async Task GetByUserNameAsync_Should_Return_Null_When_User_Does_Not_Exist()
    {
        // Act
        var result = await _repository.GetByUserNameAsync("nonexistentuser");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_User()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);
        
        var updatedUser = new User("updateduser", "updated@example.com", "updatedhash");

        // Act
        await _repository.UpdateAsync(updatedUser);

        // Assert
        var result = await _repository.GetByIdAsync(updatedUser.Id);
        result.Should().NotBeNull();
        result!.Should().Be(updatedUser);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_ArgumentNullException_When_User_Is_Null()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null!));
        exception.ParamName.Should().Be("user");
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_User()
    {
        // Arrange
        var user = new User("testuser", "test@example.com", "hashedpassword");
        await _repository.InsertAsync(user);

        // Act
        await _repository.DeleteAsync(user.Id);

        // Assert
        var result = await _repository.GetByIdAsync(user.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_User_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var action = async () => await _repository.DeleteAsync(nonExistentId);
        await action.Should().NotThrowAsync();
    }
}