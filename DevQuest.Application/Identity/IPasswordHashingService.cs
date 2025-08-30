namespace DevQuest.Application.Identity;

/// <summary>
/// Service responsible for hashing and verifying passwords using secure algorithms.
/// </summary>
public interface IPasswordHashingService
{
    /// <summary>
    /// Hashes a plain text password using SHA-256 with a random salt.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password including the salt.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hashedPassword">The stored hash to verify against.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    bool VerifyPassword(string password, string hashedPassword);
}