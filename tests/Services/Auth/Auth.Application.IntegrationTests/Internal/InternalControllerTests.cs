using Auth.Api;
using Auth.Application.IntegrationTests.TestUtils;
using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using Moq;
using Organization.Contracts.Responses;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Organization.Contracts.Requests;

namespace Auth.Application.IntegrationTests.Internal
{
    public class InternalControllerTests : BaseIntegrationTest
    {
        private readonly Mock<IOrganizationServiceClient> _orgServiceClientMock;

        public InternalControllerTests(CustomWebApplicationFactory<AuthApiMarker> factory)
            : base(factory)
        {
            // Holen uns den Mock aus der Factory, um ihn in den Tests zu konfigurieren
            _orgServiceClientMock = factory.OrganizationServiceClientMock;
        }

        [Fact]
        public async Task GetUserByEmail_Should_ReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "notfound@example.com";

            // Act
            var response = await _client.GetAsync($"/api/internal/users/by-email?email={email}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetUserByEmail_Should_ReturnUserWithDetails_WhenUserAndEmployeeExist()
        {
            // Arrange
            // 1. Benutzer in der DB anlegen
            var userEmail = "internal-test@example.com";
            var registerResponse = await _client.PostAsJsonAsync("/api/authentication/register", new RegisterUserRequest(userEmail, "Password123!"));
            var user = await registerResponse.Content.ReadFromJsonAsync<RegisterUserResponse>();

            // 2. Antwort des Organization Service mocken
            var employeeDetails = new EmployeeDetailsResponse(user.UserId, Guid.NewGuid(), "Internal", "User");
            _orgServiceClientMock.Setup(c => c.GetEmployeesByUserIdsAsync(
                                   It.Is<GetEmployeesByUserIdsRequest>(r => r.UserIds.Contains(user.UserId)),
                                   It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<EmployeeDetailsResponse> { employeeDetails });

            // Act
            var response = await _client.GetAsync($"/api/internal/users/by-email?email={userEmail}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var userDetails = await response.Content.ReadFromJsonAsync<UserDetailsResponse>();
            Assert.NotNull(userDetails);
            Assert.Equal("Internal", userDetails.FirstName);
            Assert.Equal("User", userDetails.LastName);
        }

        [Fact]
        public async Task GetUserById_Should_ReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/api/internal/users/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetUserById_Should_ReturnUser_WhenUserExists()
        {
            // Arrange
            // 1. Benutzer in der DB anlegen
            var registerResponse = await _client.PostAsJsonAsync("/api/authentication/register", new RegisterUserRequest("by-id-test@example.com", "Password123!"));
            var user = await registerResponse.Content.ReadFromJsonAsync<RegisterUserResponse>();

            // Act
            var response = await _client.GetAsync($"/api/internal/users/{user.UserId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var userDetails = await response.Content.ReadFromJsonAsync<UserDetailsResponse>();
            Assert.NotNull(userDetails);
            Assert.Equal(user.UserId, userDetails.UserId);
            Assert.Equal("by-id-test@example.com", userDetails.Email);
        }

        [Fact]
        public async Task GetUsersByIds_Should_ReturnCorrectlyMergedData()
        {
            // Arrange
            // 1. Zwei Benutzer anlegen
            var user1Response = await _client.PostAsJsonAsync("/api/authentication/register", new RegisterUserRequest("user1@internal.com", "pw"));
            var user1 = await user1Response.Content.ReadFromJsonAsync<RegisterUserResponse>();

            var user2Response = await _client.PostAsJsonAsync("/api/authentication/register", new RegisterUserRequest("user2@internal.com", "pw"));
            var user2 = await user2Response.Content.ReadFromJsonAsync<RegisterUserResponse>();

            // 2. Nur für den ersten Benutzer Details vom Organization Service mocken
            var employee1Details = new EmployeeDetailsResponse(user1.UserId, Guid.NewGuid(), "First", "User");
            _orgServiceClientMock.Setup(c => c.GetEmployeesByUserIdsAsync(
                                  It.IsAny<GetEmployeesByUserIdsRequest>(),
                                  It.IsAny<CancellationToken>()))
                                .ReturnsAsync(new List<EmployeeDetailsResponse> { employee1Details });

            var request = new GetUsersByIdsRequest(new List<Guid> { user1.UserId, user2.UserId });

            // Act
            var response = await _client.PostAsJsonAsync("/api/internal/users/by-ids", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var users = await response.Content.ReadFromJsonAsync<List<UserDetailsResponse>>();
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);

            var resultUser1 = users.First(u => u.UserId == user1.UserId);
            Assert.Equal("First", resultUser1.FirstName);

            var resultUser2 = users.First(u => u.UserId == user2.UserId);
            Assert.Null(resultUser2.FirstName);
        }

        [Fact]
        public async Task GetUsersByIds_Should_ReturnOnlyFoundUsers_WhenSomeIdsAreNotFound()
        {
            // Arrange
            // 1. Einen existierenden Benutzer anlegen
            var userResponse = await _client.PostAsJsonAsync("/api/authentication/register", new RegisterUserRequest("existing@example.com", "pw"));
            var existingUser = await userResponse.Content.ReadFromJsonAsync<RegisterUserResponse>();
            var nonExistentUserId = Guid.NewGuid();

            // 2. Mock für den Organization Service bleibt leer, da es hier nicht relevant ist
            _orgServiceClientMock.Setup(c => c.GetEmployeesByUserIdsAsync(It.IsAny<GetEmployeesByUserIdsRequest>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<EmployeeDetailsResponse>());

            var request = new GetUsersByIdsRequest(new List<Guid> { existingUser.UserId, nonExistentUserId });

            // Act
            var response = await _client.PostAsJsonAsync("/api/internal/users/by-ids", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var users = await response.Content.ReadFromJsonAsync<List<UserDetailsResponse>>();
            Assert.NotNull(users);
            // Es sollte nur der eine, tatsächlich gefundene Benutzer zurückgegeben werden
            var foundUser = Assert.Single(users);
            Assert.Equal(existingUser.UserId, foundUser.UserId);
        }

        [Fact]
        public async Task GetUsersByIds_Should_ReturnBadRequest_WhenUserIdsListIsEmpty()
        {
            // Arrange
            var request = new GetUsersByIdsRequest(new List<Guid>());

            // Act
            var response = await _client.PostAsJsonAsync("/api/internal/users/by-ids", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
