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
