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

        [Fact]
        public void CanVerifyClearTextPasswordAgainstHashedAndSaltedPassword()
        {
            // Arrange
            var clearTextPassword = "testpassword";
            var originalPasswordHash = new SaltedPasswordHash(clearTextPassword);
            var retrivedPasswordHash = new SaltedPasswordHash(originalPasswordHash.Hash, originalPasswordHash.Salt);
            
            // Act
            bool valid = retrivedPasswordHash.Verify(clearTextPassword);

            // Assert
            valid.Should().Be.True();
        }

        [Fact]
        public void CanVerifyTwoPasswordHashedCreatedFromClearTextPasswordsButTheyHashesAreNotEqual()
        {
            // Arrange
            var clearTextPassword = "testpassword";
            var hash1 = new SaltedPasswordHash(clearTextPassword);
            var hash2 = new SaltedPasswordHash(clearTextPassword);

            // Act
            bool equalHashed = hash1 == hash2;
            bool hash1VerifiesPassword = hash1.Verify(clearTextPassword);
            bool hash2VerifiesPassword = hash2.Verify(clearTextPassword);

            // Assert
            equalHashed.Should().Be.False();
            hash1VerifiesPassword.Should().Be.True();
            hash2VerifiesPassword.Should().Be.True();
        }

        [Fact]
        public void CanNotConstructWithNullPassword()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>( () =>new SaltedPasswordHash(null) );   
        }

        [Fact]
        public void CanNotConstructWithNullHash()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new SaltedPasswordHash(null, "bogus"));
        }

        [Fact]
        public void CanNotConstructWithNullSalt()
        {
            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => new SaltedPasswordHash("bogus", null));
        }
    }
}
