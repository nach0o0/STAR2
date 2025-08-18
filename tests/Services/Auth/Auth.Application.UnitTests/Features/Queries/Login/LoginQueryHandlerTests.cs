using Auth.Application.Features.Queries.Login;
using Auth.Application.Interfaces.Infrastructure;
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

namespace Auth.Application.UnitTests.Features.Queries.Login
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IBasicTokenService> _tokenServiceMock;
        private readonly LoginQueryHandler _handler;

        public LoginQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenServiceMock = new Mock<IBasicTokenService>();
            _handler = new LoginQueryHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _tokenServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var query = new LoginQuery("notfound@example.com", "password");
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenUserIsNotActive()
        {
            // Arrange
            var user = new User("inactive@example.com", "password");
            user.Deactivate(); // Deaktiviere den Benutzer
            var query = new LoginQuery(user.Email, "password");

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = new User("user@example.com", "correct_hash");
            var query = new LoginQuery(user.Email, "wrong_password");

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword(query.Password, user.PasswordHash))
                               .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ReturnLoginQueryResult_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User("user@example.com", "correct_hash");
            var query = new LoginQuery(user.Email, "correct_password");
            var expectedToken = "generated_jwt_token";

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);
            _passwordHasherMock.Setup(h => h.VerifyPassword(query.Password, user.PasswordHash))
                               .Returns(true);
            _tokenServiceMock.Setup(s => s.GenerateToken(user))
                             .Returns(expectedToken);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result.BasicToken);
        }
    }
}
