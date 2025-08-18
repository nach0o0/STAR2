using Auth.Application.Features.Queries.GetUsersByIds;
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

namespace Auth.Application.UnitTests.Features.Queries.GetUsersByIds
{
    public class GetUsersByIdsQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IOrganizationServiceClient> _orgServiceClientMock;
        private readonly GetUsersByIdsQueryHandler _handler;

        public GetUsersByIdsQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _orgServiceClientMock = new Mock<IOrganizationServiceClient>();
            _handler = new GetUsersByIdsQueryHandler(
                _userRepositoryMock.Object,
                _orgServiceClientMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_WhenNoUsersAreFound()
        {
            // Arrange
            var query = new GetUsersByIdsQuery(new List<Guid> { Guid.NewGuid() });
            _userRepositoryMock.Setup(r => r.GetByIdsAsync(query.UserIds, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new List<User>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_Should_ReturnUsersWithNullNames_WhenOrganizationServiceReturnsNoDetails()
        {
            // Arrange
            var user1 = new User("user1@example.com", "pw");
            var user2 = new User("user2@example.com", "pw");
            var query = new GetUsersByIdsQuery(new List<Guid> { user1.Id, user2.Id });

            _userRepositoryMock.Setup(r => r.GetByIdsAsync(query.UserIds, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new List<User> { user1, user2 });

            _orgServiceClientMock.Setup(c => c.GetEmployeesByUserIdsAsync(It.IsAny<GetEmployeesByUserIdsRequest>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<EmployeeDetailsResponse>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Null(item.FirstName));
            Assert.All(result, item => Assert.Null(item.LastName));
        }

        [Fact]
        public async Task Handle_Should_CorrectlyMergeUserDetails_WhenAllDataIsPresent()
        {
            // Arrange
            var user1 = new User("user1@example.com", "pw");
            var user2 = new User("user2@example.com", "pw"); // Dieser User hat keine Employee-Details
            var query = new GetUsersByIdsQuery(new List<Guid> { user1.Id, user2.Id });

            var employee1Details = new EmployeeDetailsResponse(user1.Id, Guid.NewGuid(), "Peter", "Pan");

            _userRepositoryMock.Setup(r => r.GetByIdsAsync(query.UserIds, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new List<User> { user1, user2 });

            _orgServiceClientMock.Setup(c => c.GetEmployeesByUserIdsAsync(
                                   It.Is<GetEmployeesByUserIdsRequest>(r => r.UserIds.SequenceEqual(query.UserIds)),
                                   It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<EmployeeDetailsResponse> { employee1Details });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);

            var resultUser1 = result.First(u => u.UserId == user1.Id);
            Assert.Equal("Peter", resultUser1.FirstName);
            Assert.Equal("Pan", resultUser1.LastName);

            var resultUser2 = result.First(u => u.UserId == user2.Id);
            Assert.Null(resultUser2.FirstName);
            Assert.Null(resultUser2.LastName);
        }
    }
}
