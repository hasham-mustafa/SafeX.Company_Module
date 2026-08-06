using SafeX.CompanyPanel.Helpers;
using Xunit;

namespace SafeX.CompanyPanel.Tests.Helpers
{
    public class PasswordHelperTests
    {
        [Fact]
        public void HashPassword_DoesNotReturnPlainText()
        {
            // Arrange
            const string plainPassword = "SuperSecret123!";

            // Act
            var hash = PasswordHelper.HashPassword(plainPassword);

            // Assert
            Assert.NotEqual(plainPassword, hash);
            Assert.False(string.IsNullOrWhiteSpace(hash));
        }

        [Fact]
        public void HashPassword_ProducesDifferentHashes_ForSamePasswordEachTime()
        {
            // BCrypt generates a random salt per call, so two hashes of the
            // same password must never be identical. This guards against
            // someone "optimizing" HashPassword into something deterministic.
            const string plainPassword = "SuperSecret123!";

            var hash1 = PasswordHelper.HashPassword(plainPassword);
            var hash2 = PasswordHelper.HashPassword(plainPassword);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_ReturnsTrue_ForCorrectPassword()
        {
            const string plainPassword = "SuperSecret123!";
            var hash = PasswordHelper.HashPassword(plainPassword);

            var result = PasswordHelper.VerifyPassword(plainPassword, hash);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ReturnsFalse_ForIncorrectPassword()
        {
            var hash = PasswordHelper.HashPassword("SuperSecret123!");

            var result = PasswordHelper.VerifyPassword("WrongPassword!", hash);

            Assert.False(result);
        }

        [Fact]
        public void GenerateResetToken_ReturnsUniqueTokenEachCall()
        {
            var token1 = PasswordHelper.GenerateResetToken();
            var token2 = PasswordHelper.GenerateResetToken();

            Assert.NotEqual(token1, token2);
            Assert.False(string.IsNullOrWhiteSpace(token1));
        }
    }
}
