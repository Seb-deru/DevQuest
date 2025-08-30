using DevQuest.Domain.Identity;

namespace DevQuest.Domain.Tests.Identity;

public class UserTests
{
    [Fact]
    public void User_Constructor_Should_Create_User_With_Valid_Properties()
    {
        // Arrange
        const string userName = "testuser";
        const string email = "test@example.com";
        const string passwordHash = "hashedpassword123";

        // Act
        var user = new User(userName, email, passwordHash);

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().NotBeEmpty();
        user.UserName.Should().Be(userName);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
    }

    [Fact]
    public void User_Should_Generate_Unique_Ids()
    {
        // Arrange & Act
        var user1 = new User("user1", "user1@example.com", "hash1");
        var user2 = new User("user2", "user2@example.com", "hash2");

        // Assert
        user1.Id.Should().NotBe(user2.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("   ")]
    public void User_Constructor_Should_Throw_ArgumentException_When_UserName_Is_Invalid(string userName)
    {
        // Arrange
        const string email = "test@example.com";
        const string passwordHash = "hashedpassword123";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new User(userName, email, passwordHash));
        exception.ParamName.Should().Be("userName");
        exception.Message.Should().Contain("Username cannot be null, empty, or whitespace.");
    }

    [Fact]
    public void User_Constructor_Should_Create_User_With_Valid_UserName()
    {
        // Arrange
        const string userName = "validuser";
        const string email = "test@example.com";
        const string passwordHash = "hashedpassword123";

        // Act
        var user = new User(userName, email, passwordHash);

        // Assert
        user.UserName.Should().Be(userName);
    }
}