using DevQuest.Application.Identity;
using DevQuest.Application.Identity.CreateUser;
using DevQuest.Domain.Identity;

namespace DevQuest.Application.Tests.Identity.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHashingService> _passwordHashingServiceMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHashingServiceMock = new Mock<IPasswordHashingService>();
        _handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _passwordHashingServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_User_Successfully_When_Request_Is_Valid()
    {
        // Arrange
        var request = new CreateUserRequest("testuser", "test@example.com", "password123");
        var hashedPassword = "salt:hashedpassword";
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);
        
        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(request.UserName))
            .ReturnsAsync((User?)null);

        _passwordHashingServiceMock.Setup(x => x.HashPassword(request.Password))
            .Returns(hashedPassword);

        // Act
        var result = await _handler.Handle(request);

        // Assert
        result.Should().NotBeNull();
        result.UserName.Should().Be(request.UserName);
        result.Email.Should().Be(request.Email);
        result.Id.Should().NotBeEmpty();

        _userRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<User>()), Times.Once);
        _passwordHashingServiceMock.Verify(x => x.HashPassword(request.Password), Times.Once);
    }

    [Theory]
    [InlineData("", "test@example.com", "password123", "UserName")]
    [InlineData("   ", "test@example.com", "password123", "UserName")]
    [InlineData(null, "test@example.com", "password123", "UserName")]
    [InlineData("testuser", "", "password123", "Email")]
    [InlineData("testuser", "   ", "password123", "Email")]
    [InlineData("testuser", null, "password123", "Email")]
    [InlineData("testuser", "test@example.com", "", "Password")]
    [InlineData("testuser", "test@example.com", "   ", "Password")]
    [InlineData("testuser", "test@example.com", null, "Password")]
    public async Task Handle_Should_Throw_ArgumentException_When_Required_Fields_Are_Empty(
        string userName, string email, string password, string expectedParameterName)
    {
        // Arrange
        var request = new CreateUserRequest(userName, email, password);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request));
        exception.ParamName.Should().Be(expectedParameterName);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_Email_Already_Exists()
    {
        // Arrange
        var request = new CreateUserRequest("testuser", "test@example.com", "password123");
        var existingUser = new User("existinguser", "test@example.com", "existinghash");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request));
        exception.Message.Should().Contain($"User with email '{request.Email}' already exists");
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperationException_When_UserName_Already_Exists()
    {
        // Arrange
        var request = new CreateUserRequest("testuser", "test@example.com", "password123");
        var existingUser = new User("testuser", "existing@example.com", "existinghash");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);
        
        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(request.UserName))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request));
        exception.Message.Should().Contain($"User with username '{request.UserName}' already exists");
    }

    [Fact]
    public async Task Handle_Should_Hash_Password_Before_Storing()
    {
        // Arrange
        var request = new CreateUserRequest("testuser", "test@example.com", "password123");
        var hashedPassword = "salt:hashedpassword";
        User? capturedUser = null;

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);
        
        _userRepositoryMock.Setup(x => x.GetByUserNameAsync(request.UserName))
            .ReturnsAsync((User?)null);

        _passwordHashingServiceMock.Setup(x => x.HashPassword(request.Password))
            .Returns(hashedPassword);
        
        _userRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<User>()))
            .Callback<User>(user => capturedUser = user);

        // Act
        await _handler.Handle(request);

        // Assert
        capturedUser.Should().NotBeNull();
        capturedUser!.PasswordHash.Should().NotBe(request.Password);
        capturedUser.PasswordHash.Should().Be(hashedPassword);

        _passwordHashingServiceMock.Verify(x => x.HashPassword(request.Password), Times.Once);
    }
}