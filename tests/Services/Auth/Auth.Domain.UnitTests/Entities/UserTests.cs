using Auth.Domain.Entities;
using Auth.Domain.Events.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.UnitTests.Entities
{
    public class UserTests
    {
        private const string TestEmail = "test@example.com";
        private const string InitialPasswordHash = "initial_hash";
        private const string NewPasswordHash = "new_stronger_hash";

        [Fact]
        public void Constructor_Should_CreateUser_WithCorrectInitialState()
        {
            // Arrange & Act
            var user = new User(TestEmail, InitialPasswordHash);

            // Assert
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal(TestEmail, user.Email);
            Assert.Equal(InitialPasswordHash, user.PasswordHash);
            Assert.True(user.IsActive);
            Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 1);
            Assert.Empty(user.GetDomainEvents());
        }

        [Fact]
        public void ChangePassword_Should_UpdatePasswordHash_And_RaiseDomainEvent()
        {
            // Arrange
            var user = new User(TestEmail, InitialPasswordHash);

            // Act
            user.ChangePassword(NewPasswordHash);

            // Assert
            Assert.Equal(NewPasswordHash, user.PasswordHash);
            Assert.NotNull(user.UpdatedAt);
            Assert.True((DateTime.UtcNow - user.UpdatedAt.Value).TotalSeconds < 1);

            var domainEvent = Assert.Single(user.GetDomainEvents());
            var passwordChangedEvent = Assert.IsType<UserPasswordChangedEvent>(domainEvent);
            Assert.Equal(user.Id, passwordChangedEvent.UserId);
        }

        [Fact]
        public void Deactivate_Should_SetIsActiveToFalse()
        {
            // Arrange
            var user = new User(TestEmail, InitialPasswordHash);

            // Act
            user.Deactivate();

            // Assert
            Assert.False(user.IsActive);
            Assert.NotNull(user.UpdatedAt);
        }

        [Fact]
        public void Activate_Should_SetIsActiveToTrue()
        {
            // Arrange
            var user = new User(TestEmail, InitialPasswordHash);
            user.Deactivate(); // Start with a deactivated user

            // Act
            user.Activate();

            // Assert
            Assert.True(user.IsActive);
            Assert.NotNull(user.UpdatedAt);
        }

        [Fact]
        public void PrepareForDeletion_Should_RaiseUserAccountDeletedEvent()
        {
            // Arrange
            var user = new User(TestEmail, InitialPasswordHash);

            // Act
            user.PrepareForDeletion();

            // Assert
            var domainEvent = Assert.Single(user.GetDomainEvents());
            var deletedEvent = Assert.IsType<UserAccountDeletedEvent>(domainEvent);
            Assert.Equal(user.Id, deletedEvent.UserId);
        }



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_Should_ThrowArgumentException_WhenEmailIsInvalid(string invalidEmail)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new User(invalidEmail, InitialPasswordHash));
        }

        [Fact]
        public void Deactivate_Should_DoNothing_WhenUserIsAlreadyDeactivated()
        {
            // Arrange
            var user = new User(TestEmail, InitialPasswordHash);
            user.Deactivate();
            user.ClearDomainEvents(); // Events von der ersten Deaktivierung löschen

            // Act
            user.Deactivate();

            // Assert
            Assert.False(user.IsActive);
            Assert.Empty(user.GetDomainEvents()); // Wichtig: Es sollte kein neues Event geben
        }
    }
}
