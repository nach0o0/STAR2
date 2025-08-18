using Auth.Application.Features.Commands.PrivilegedResetPassword;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using Moq;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.PrivilegedResetPassword
{
    public class PrivilegedResetPasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly PrivilegedResetPasswordCommandHandler _handler;

        public PrivilegedResetPasswordCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new PrivilegedResetPasswordCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new PrivilegedResetPasswordCommand(Guid.NewGuid(), "new_password");
            _userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_CallChangePasswordOnUser_WhenUserExists()
        {
            // Arrange
            var user = new User("test@test.com", "old_hash");
            var command = new PrivilegedResetPasswordCommand(user.Id, "new_password");
            var newHashedPassword = "new_hashed_password";

            _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.HashPassword(command.NewPassword))
                               .Returns(newHashedPassword);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newHashedPassword, user.PasswordHash);
            Assert.Single(user.GetDomainEvents()); // Stellt sicher, dass das UserPasswordChangedEvent ausgelöst wurde
        }
    }
}
