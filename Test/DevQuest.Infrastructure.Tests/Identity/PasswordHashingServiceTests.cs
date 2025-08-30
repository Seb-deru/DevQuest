using DevQuest.Application.Identity;
using DevQuest.Infrastructure.Identity;

namespace DevQuest.Infrastructure.Tests.Identity;

public class PasswordHashingServiceTests
{
    private readonly PasswordHashingService _passwordHashingService;

    public PasswordHashingServiceTests()
    {
        _passwordHashingService = new PasswordHashingService();
    }

    [Fact]
    public void HashPassword_Should_Return_Different_Hashes_For_Same_Password()
    {
        // Arrange
        const string password = "testpassword123";

        // Act
        var hash1 = _passwordHashingService.HashPassword(password);
        var hash2 = _passwordHashingService.HashPassword(password);

        // Assert
        hash1.Should().NotBe(hash2);
        hash1.Should().Contain(":");
        hash2.Should().Contain(":");
    }

    [Fact]
    public void HashPassword_Should_Return_Base64_Encoded_Salt_And_Hash()
    {
        // Arrange
        const string password = "testpassword123";

        // Act
        var result = _passwordHashingService.HashPassword(password);

        // Assert
        var parts = result.Split(':');
        parts.Should().HaveCount(2);

        // Both parts should be valid base64 strings
        var saltBytes = Convert.FromBase64String(parts[0]);
        var hashBytes = Convert.FromBase64String(parts[1]);

        saltBytes.Should().HaveCount(32); // 256 bits salt
        hashBytes.Should().HaveCount(32); // SHA-256 produces 256 bits = 32 bytes
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void HashPassword_Should_Throw_ArgumentException_When_Password_Is_Invalid(string password)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _passwordHashingService.HashPassword(password));
        exception.ParamName.Should().Be("password");
    }

    [Fact]
    public void VerifyPassword_Should_Return_True_For_Correct_Password()
    {
        // Arrange
        const string password = "testpassword123";
        var hashedPassword = _passwordHashingService.HashPassword(password);

        // Act
        var result = _passwordHashingService.VerifyPassword(password, hashedPassword);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_Should_Return_False_For_Incorrect_Password()
    {
        // Arrange
        const string correctPassword = "testpassword123";
        const string incorrectPassword = "wrongpassword";
        var hashedPassword = _passwordHashingService.HashPassword(correctPassword);

        // Act
        var result = _passwordHashingService.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "salt:hash")]
    [InlineData(null, "salt:hash")]
    public void VerifyPassword_Should_Throw_ArgumentException_When_Password_Is_Invalid(string password, string hashedPassword)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _passwordHashingService.VerifyPassword(password, hashedPassword));
        exception.ParamName.Should().Be("password");
    }

    [Theory]
    [InlineData("password123", "")]
    [InlineData("password123", null)]
    public void VerifyPassword_Should_Throw_ArgumentException_When_HashedPassword_Is_Invalid(string password, string hashedPassword)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _passwordHashingService.VerifyPassword(password, hashedPassword));
        exception.ParamName.Should().Be("hashedPassword");
    }

    [Theory]
    [InlineData("password123", "invalidsalt")]
    [InlineData("password123", "salt")]
    [InlineData("password123", "salt:hash:extra")]
    [InlineData("password123", "invalidbase64!:hash")]
    [InlineData("password123", "salt:invalidbase64!")]
    public void VerifyPassword_Should_Return_False_For_Malformed_Hash(string password, string malformedHash)
    {
        // Act
        var result = _passwordHashingService.VerifyPassword(password, malformedHash);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void VerifyPassword_Should_Work_With_Different_Passwords_And_Hashes()
    {
        // Arrange
        var passwords = new[] { "password1", "password2", "password3", "veryLongPasswordWithSpecialChars!@#$%^&*()" };
        var hashes = passwords.Select(p => _passwordHashingService.HashPassword(p)).ToArray();

        // Act & Assert
        for (int i = 0; i < passwords.Length; i++)
        {
            // Correct password should verify
            _passwordHashingService.VerifyPassword(passwords[i], hashes[i]).Should().BeTrue();

            // Wrong passwords should not verify
            for (int j = 0; j < passwords.Length; j++)
            {
                if (i != j)
                {
                    _passwordHashingService.VerifyPassword(passwords[i], hashes[j]).Should().BeFalse();
                }
            }
        }
    }
}