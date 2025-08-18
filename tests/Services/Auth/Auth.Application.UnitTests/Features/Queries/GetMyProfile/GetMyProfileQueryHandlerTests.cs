using Auth.Application.Features.Queries.GetMyProfile;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using Moq;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.GetMyProfile
{
    public class GetMyProfileQueryHandlerTests
    {
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetMyProfileQueryHandler _handler;

        public GetMyProfileQueryHandlerTests()
        {
            _userContextMock = new Mock<IUserContext>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetMyProfileQueryHandler(_userContextMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ThrowUnauthorizedAccessException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _userContextMock.Setup(c => c.GetCurrentUser()).Returns((CurrentUser)null);
            var query = new GetMyProfileQuery();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenUserIsAuthenticatedButNotFoundInRepository()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentUser = new CurrentUser(userId, null, null, new List<Guid>(), new Dictionary<string, List<string>>());
            _userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            var query = new GetMyProfileQuery();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ReturnCorrectProfile_WhenUserIsAuthenticatedAndExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEmail = "test@example.com";
            var user = new User(userEmail, "hashed_password");
            var currentUser = new CurrentUser(userId, null, null, new List<Guid>(), new Dictionary<string, List<string>>());

            _userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            var query = new GetMyProfileQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
        }
    }
}
