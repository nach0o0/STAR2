using Auth.Application.Features.Queries.GetUserByEmail;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Entities;
using Moq;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UnitTests.Features.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IOrganizationServiceClient> _orgServiceClientMock;
        private readonly GetUserByEmailQueryHandler _handler;

        public GetUserByEmailQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _orgServiceClientMock = new Mock<IOrganizationServiceClient>();
            _handler = new GetUserByEmailQueryHandler(
                _userRepositoryMock.Object,
                _orgServiceClientMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnNull_WhenUserNotFound()
        {
            // Arrange
            var query = new GetUserByEmailQuery("notfound@example.com");
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_Should_ReturnUserWithNullNames_WhenEmployeeDetailsAreNotFound()
        {
            // Arrange
            var user = new User("user@example.com", "password");
            var query = new GetUserByEmailQuery(user.Email);

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            _orgServiceClientMock.Setup(c => c.GetEmployeesByUserIdsAsync(
                                   It.Is<GetEmployeesByUserIdsRequest>(r => r.UserIds.Contains(user.Id)),
                                   It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<EmployeeDetailsResponse>()); // Leere Liste zurückgeben

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
            Assert.Null(result.FirstName);
            Assert.Null(result.LastName);
        }

        [Fact]
        public async Task Handle_Should_ReturnUserWithFullDetails_WhenEmployeeDetailsAreFound()
        {
            // Arrange
            var user = new User("user@example.com", "password");
            var query = new GetUserByEmailQuery(user.Email);
            var employeeDetails = new EmployeeDetailsResponse(user.Id, Guid.NewGuid(), "John", "Doe");

            _userRepositoryMock.Setup(r => r.GetByEmailAsync(query.Email, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            _orgServiceClientMock.Setup(c => c.GetEmployeesByUserIdsAsync(
                                   It.Is<GetEmployeesByUserIdsRequest>(r => r.UserIds.Contains(user.Id)),
                                   It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<EmployeeDetailsResponse> { employeeDetails });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }
    }
}
