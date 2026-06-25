namespace SewingSystem.Classes
{
    /// <summary>
    /// Central password hashing/verification (BCrypt).
    ///
    /// User management already stores new/changed passwords as BCrypt hashes,
    /// but login historically compared plaintext — so any user created through
    /// the UI could never sign in. <see cref="Verify"/> fixes that and stays
    /// backward compatible with legacy plaintext rows (e.g. the seeded Admin):
    /// a stored BCrypt hash is verified with BCrypt, anything else is treated as
    /// legacy plaintext and compared directly.
    /// </summary>
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password ?? string.Empty);
        }

        /// <summary>True if <paramref name="enteredPassword"/> matches the stored value.</summary>
        public static bool Verify(string enteredPassword, string storedPassword)
        {
            if (storedPassword == null)
                return false;

            // BCrypt hashes start with the version tag $2a$ / $2b$ / $2y$.
            if (storedPassword.StartsWith("$2"))
            {
                try
                {
                    return BCrypt.Net.BCrypt.Verify(enteredPassword ?? string.Empty, storedPassword);
                }
                catch
                {
                    return false;
                }
            }

            // Legacy plaintext row (predates hashing).
            return storedPassword == enteredPassword;
        }

        /// <summary>True when a stored value is still legacy plaintext (candidate for re-hashing).</summary>
        public static bool IsLegacyPlaintext(string storedPassword)
        {
            return !string.IsNullOrEmpty(storedPassword) && !storedPassword.StartsWith("$2");
        }
    }
}
