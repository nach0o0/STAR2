using Auth.Api;
using Auth.Application.IntegrationTests.TestUtils;
using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain.Authorization;
using System.Net.Http.Headers;

namespace Auth.Application.IntegrationTests.Users
{
    public class UsersControllerTests : BaseIntegrationTest
    {
        public UsersControllerTests(CustomWebApplicationFactory<AuthApiMarker> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetMyProfile_Should_ReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            // Wir verwenden den Standard-Client ohne Authentifizierung

            // Act
            var response = await _client.GetAsync("/api/users/me");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMyProfile_Should_ReturnOkWithProfile_WhenUserIsAuthenticated()
        {
            // Arrange
            var userEmail = "profile-user@example.com";
            var authenticatedClient = await CreateAuthenticatedClientAsync(userEmail);

            // Act
            var response = await authenticatedClient.GetAsync("/api/users/me");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var profile = await response.Content.ReadFromJsonAsync<MyProfileResponse>();
            Assert.NotNull(profile);
            Assert.Equal(userEmail, profile.Email);
        }

        [Fact]
        public async Task ChangePassword_Should_ReturnNoContent_WhenRequestIsValid()
        {
            // Arrange
            var userEmail = "change-pw@example.com";
            var oldPassword = "OldPassword123!";
            var newPassword = "NewPassword456!";
            var authenticatedClient = await CreateAuthenticatedClientAsync(userEmail, oldPassword);

            var request = new ChangePasswordRequest(oldPassword, newPassword);

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/users/me/change-password", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Optional: Versuchen, sich mit dem neuen Passwort anzumelden, um die Änderung zu bestätigen
            var loginRequest = new LoginRequest(userEmail, newPassword);
            var loginResponse = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_Should_ReturnBadRequest_WhenOldPasswordIsIncorrect()
        {
            // Arrange
            var userEmail = "change-pw-fail@example.com";
            var correctOldPassword = "OldPassword123!";
            var authenticatedClient = await CreateAuthenticatedClientAsync(userEmail, correctOldPassword);
            var request = new ChangePasswordRequest("this-is-the-wrong-password", "NewPassword456!");

            // Act
            var response = await authenticatedClient.PostAsJsonAsync("/api/users/me/change-password", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteMyAccount_Should_ReturnNoContent_WhenPasswordIsCorrect()
        {
            // Arrange
            var userEmail = "delete-me@example.com";
            var password = "Password123!";
            var authenticatedClient = await CreateAuthenticatedClientAsync(userEmail, password);

            var request = new DeleteMyAccountRequest(password);

            // Act
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/users/me")
            {
                Content = Utilities.GetRequestContent(request)
            };
            var response = await authenticatedClient.SendAsync(httpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Optional: Überprüfen, ob der Login danach fehlschlägt
            var loginRequest = new LoginRequest(userEmail, password);
            var loginResponse = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);
            Assert.Equal(HttpStatusCode.NotFound, loginResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteMyAccount_Should_ReturnBadRequest_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userEmail = "delete-me-fail@example.com";
            var correctPassword = "Password123!";
            var authenticatedClient = await CreateAuthenticatedClientAsync(userEmail, correctPassword);
            var request = new DeleteMyAccountRequest("this-is-the-wrong-password");

            // Act
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/users/me")
            {
                Content = Utilities.GetRequestContent(request)
            };
            var response = await authenticatedClient.SendAsync(httpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PrivilegedResetPassword_Should_ReturnNoContent_WhenAdminHasGlobalPermission()
        {
            // Arrange
            // 1. Erstelle einen Ziel-Benutzer, dessen Passwort zurückgesetzt werden soll
            var targetUserEmail = "target-user@example.com";
            var targetUserPassword = "OldPassword123!";
            var registerResponse = await _client.PostAsJsonAsync("/api/authentication/register", new RegisterUserRequest(targetUserEmail, targetUserPassword));
            var targetUser = await registerResponse.Content.ReadFromJsonAsync<RegisterUserResponse>();

            // 2. Erstelle einen Admin-Benutzer mit der notwendigen globalen Berechtigung
            var adminId = System.Guid.NewGuid();
            var permissions = new Dictionary<string, List<string>>
            {
                { PermittedScopeTypes.Global, new List<string> { "user:privileged-reset-password" } }
            };
            var adminToken = _factory.GenerateTestJwtToken(adminId, "admin@example.com", permissions);

            var adminClient = _factory.CreateClient();
            adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new PrivilegedResetPasswordRequest("NewStrongPassword456!");

            // Act
            var response = await adminClient.PostAsJsonAsync($"/api/users/{targetUser.UserId}/privileged-reset-password", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Optional: Prüfen, ob der Login mit dem alten Passwort fehlschlägt und mit dem neuen funktioniert
            var failedLoginResponse = await _client.PostAsJsonAsync("/api/authentication/login", new LoginRequest(targetUserEmail, targetUserPassword));
            Assert.Equal(HttpStatusCode.NotFound, failedLoginResponse.StatusCode);

            var successfulLoginResponse = await _client.PostAsJsonAsync("/api/authentication/login", new LoginRequest(targetUserEmail, "NewStrongPassword456!"));
            Assert.Equal(HttpStatusCode.OK, successfulLoginResponse.StatusCode);
        }

        [Fact]
        public async Task PrivilegedResetPassword_Should_ReturnForbidden_WhenUserLacksPermission()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();

            // Erstelle einen normalen Benutzer OHNE die Admin-Berechtigung
            var regularUserId = Guid.NewGuid();
            var regularToken = _factory.GenerateTestJwtToken(regularUserId, "regular@example.com", new Dictionary<string, List<string>>());

            var regularClient = _factory.CreateClient();
            regularClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", regularToken);

            var request = new PrivilegedResetPasswordRequest("NewPassword123!");

            // Act
            var response = await regularClient.PostAsJsonAsync($"/api/users/{targetUserId}/privileged-reset-password", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
