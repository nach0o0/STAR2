using Auth.Application.Features.Queries.GetUserById;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.GetUserById
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnNull_WhenUserNotFound()
        {
            // Arrange
            var query = new GetUserByIdQuery(Guid.NewGuid());
            _userRepositoryMock.Setup(r => r.GetByIdAsync(query.UserId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_Should_ReturnUser_WhenUserIsFound()
        {
            // Arrange
            var user = new User("found@example.com", "password");
            var query = new GetUserByIdQuery(user.Id);

            _userRepositoryMock.Setup(r => r.GetByIdAsync(query.UserId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
        }
    }
}
