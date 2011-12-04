using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Should.Fluent;
using System.Security.Cryptography;

namespace SimplePassword.Tests
{
    class CreateAndVerifyPassword
    {
        [Fact]
        public void CanVerifyPasswordCreatedWithClearTextAgainstPasswordCreatedFromCorrectHashAndSalt()
        {
            // Arrange
            var password = "testpassword";
            
            // Act
            var passwordHash = new SaltedPasswordHash(password);
            var verificationHash = new SaltedPasswordHash(passwordHash.Hash, passwordHash.Salt);
            bool valid = verificationHash == passwordHash;

            // Assert
            valid.Should().Be.True();
        }

        [Fact]
        public void CanNotVerifyTwoIdenticalPasswordsCreatedFromClearText()
        {
            // Arrange
            var password = "testpassword";
            var password2 = "testpassword";

            // Act
            var passwordHash1 = new SaltedPasswordHash(password);
            var passwordHash2 = new SaltedPasswordHash(password2);

            bool valid = passwordHash1 == passwordHash2;

            // Assert
            valid.Should().Be.False();
        }

        [Fact]
        public void DoesNotVerifyIncorrectPassword()
        {
            // Arrange
            var password = "testpassword";
            var password2 = "boguspassword";

            // Act
            var passwordHash1 = new SaltedPasswordHash(password);
            var passwordHash2 = new SaltedPasswordHash(password2);

            bool valid = passwordHash1 == passwordHash2;

            // Assert
            valid.Should().Be.False();
        }

        [Fact]
        public void CanDetermineIfHashedNotEqual()
        {
            // Arrange
            var password = "testpassword";
            var password2 = "boguspassword";

            // Act
            var passwordHash1 = new SaltedPasswordHash(password);
            var passwordHash2 = new SaltedPasswordHash(password2);

            bool valid = passwordHash1 != passwordHash2;

            // Assert
            valid.Should().Be.True();
        }
    }
}
