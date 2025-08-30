using DevQuest.Application.Identity;
using DevQuest.Application.Identity.GetUserById;
using DevQuest.Domain.Identity;

namespace DevQuest.Application.Tests.Identity.GetUserById;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_User_When_User_Exists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("testuser", "test@example.com", "hashedpassword");
        
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(userId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_User_Does_Not_Exist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Call_Repository_GetByIdAsync_Once()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        await _handler.Handle(userId);

        // Assert
        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
    }
}