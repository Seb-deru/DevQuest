using System.Security.Cryptography;
using System.Text;
using DevQuest.Application.Identity;

namespace DevQuest.Infrastructure.Identity;

/// <summary>
/// Password hashing service implementation using SHA-256 with salt for secure password storage.
/// </summary>
public class PasswordHashingService : IPasswordHashingService
{
    private const int SaltSize = 32; // 256 bits
    private const char Delimiter = ':';

    /// <summary>
    /// Hashes a plain text password using SHA-256 with a random salt.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password in format: salt:hash (both base64 encoded).</returns>
    /// <exception cref="ArgumentException">Thrown when password is null or empty.</exception>
    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        // Generate a random salt
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password with the salt
        byte[] hash = HashPasswordWithSalt(password, salt);

        // Return salt:hash format (both base64 encoded)
        return Convert.ToBase64String(salt) + Delimiter + Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hashedPassword">The stored hash in format: salt:hash.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown when password or hashedPassword is invalid.</exception>
    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        if (string.IsNullOrEmpty(hashedPassword))
            throw new ArgumentException("Hashed password cannot be null or empty.", nameof(hashedPassword));

        try
        {
            // Split the stored hash into salt and hash parts
            var parts = hashedPassword.Split(Delimiter);
            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHash = Convert.FromBase64String(parts[1]);

            // Hash the provided password with the stored salt
            byte[] computedHash = HashPasswordWithSalt(password, salt);

            // Compare the computed hash with the stored hash
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
        catch
        {
            // If any exception occurs during verification, return false
            return false;
        }
    }

    /// <summary>
    /// Hashes a password with the provided salt using SHA-256.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <param name="salt">The salt bytes.</param>
    /// <returns>The hashed password bytes.</returns>
    private static byte[] HashPasswordWithSalt(string password, byte[] salt)
    {
        using var sha256 = SHA256.Create();
        
        // Combine password bytes with salt
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];
        
        Array.Copy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
        Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

        // Hash the salted password
        return sha256.ComputeHash(saltedPassword);
    }
}