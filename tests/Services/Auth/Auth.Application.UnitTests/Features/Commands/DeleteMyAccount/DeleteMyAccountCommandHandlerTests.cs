using Auth.Application.Features.Commands.DeleteMyAccount;
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

namespace Auth.Application.UnitTests.Features.Commands.DeleteMyAccount
{
    public class DeleteMyAccountCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly DeleteMyAccountCommandHandler _handler;

        public DeleteMyAccountCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userContextMock = new Mock<IUserContext>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new DeleteMyAccountCommandHandler(
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
            var command = new DeleteMyAccountCommand("any_password");

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
            var command = new DeleteMyAccountCommand("any_password");

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowValidationException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("test@test.com", "correct_hashed_password");
            var currentUser = new CurrentUser(userId, null, null, new List<Guid>(), new Dictionary<string, List<string>>());
            var command = new DeleteMyAccountCommand("wrong_password");

            _userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword(command.Password, user.PasswordHash)).Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.True(exception.Errors.ContainsKey("Password"));
        }

        [Fact]
        public async Task Handle_Should_CallPrepareForDeletionAndRepositoryDelete_WhenCommandIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("test@test.com", "correct_hashed_password");
            var currentUser = new CurrentUser(userId, null, null, new List<Guid>(), new Dictionary<string, List<string>>());
            var command = new DeleteMyAccountCommand("correct_password");

            _userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword(command.Password, user.PasswordHash)).Returns(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Single(user.GetDomainEvents()); // Prüft, ob PrepareForDeletion aufgerufen wurde und das Event ausgelöst hat
            _userRepositoryMock.Verify(r => r.Delete(user), Times.Once); // Prüft, ob der Repository-Aufruf erfolgt ist
        }
    }
}
