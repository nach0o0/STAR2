using Auth.Application.Features.Commands.RegisterUser;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using Moq;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Commands.RegisterUser
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new RegisterUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_HashPasswordAndAddUserToRepository()
        {
            // Arrange
            var command = new RegisterUserCommand("test@example.com", "password123");
            var hashedPassword = "hashed_password_abc123";

            _passwordHasherMock.Setup(h => h.HashPassword(command.Password))
                               .Returns(hashedPassword);

            // Act
            var userId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, userId);

            // Überprüft, ob AddAsync mit einem User-Objekt aufgerufen wurde, das die korrekten Daten enthält
            _userRepositoryMock.Verify(r => r.AddAsync(
                It.Is<User>(u => u.Email == command.Email && u.PasswordHash == hashedPassword),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
