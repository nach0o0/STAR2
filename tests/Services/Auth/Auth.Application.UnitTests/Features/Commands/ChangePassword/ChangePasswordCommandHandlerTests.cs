using Auth.Application.Features.Commands.ChangePassword;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using Moq;
using Shared.Application.Exceptions;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.ChangePassword
{
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userContextMock = new Mock<IUserContext>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new ChangePasswordCommandHandler(
                _userRepositoryMock.Object,
                _userContextMock.Object,
                _passwordHasherMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ThrowUnauthorizedAccessException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _userContextMock.Setup(c => c.GetCurrentUser()).Returns((CurrentUser)null);
            var command = new ChangePasswordCommand("old", "new");

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentUser = new CurrentUser(userId, null, null, new List<Guid>(), new Dictionary<string, List<string>>());
            _userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            var command = new ChangePasswordCommand("old", "new");

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowValidationException_WhenOldPasswordIsIncorrect()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("test@test.com", "old_hashed_password");
            var currentUser = new CurrentUser(userId, null, null, new List<Guid>(), new Dictionary<string, List<string>>());

            _userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword("wrong_old_password", user.PasswordHash)).Returns(false);

            var command = new ChangePasswordCommand("wrong_old_password", "new_password");

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_CallChangePasswordOnUser_WhenCommandIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("test@test.com", "old_hashed_password");
            var currentUser = new CurrentUser(userId, null, null, new List<Guid>(), new Dictionary<string, List<string>>());
            var command = new ChangePasswordCommand("correct_old_password", "new_valid_password");

            _userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword(command.OldPassword, user.PasswordHash)).Returns(true);
            _passwordHasherMock.Setup(h => h.HashPassword(command.NewPassword)).Returns("new_hashed_password");

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("new_hashed_password", user.PasswordHash);
            Assert.Single(user.GetDomainEvents()); // Prüft, ob das Event ausgelöst wurde
        }
    }
}
