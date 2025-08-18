using Auth.Api;
using Auth.Application.IntegrationTests.TestUtils;
using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using Auth.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.IntegrationTests.Authentication
{
    public class AuthenticationControllerTests : BaseIntegrationTest
    {
        public AuthenticationControllerTests(CustomWebApplicationFactory<AuthApiMarker> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Register_Should_ReturnCreated_And_CreateUser_WhenRequestIsValid()
        {
            // Arrange
            var request = new RegisterUserRequest("test@example.com", "Password123!");

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/register", request);

            // Assert
            // 1. Überprüfe die HTTP-Antwort
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var registerResponse = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();
            Assert.NotNull(registerResponse);
            Assert.NotEqual(System.Guid.Empty, registerResponse.UserId);

            // 2. Überprüfe direkt in der (In-Memory-)Datenbank, ob der Benutzer angelegt wurde
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                var userInDb = await dbContext.Users.FindAsync(registerResponse.UserId);

                Assert.NotNull(userInDb);
                Assert.Equal(request.Email, userInDb.Email);
            }
        }

        [Fact]
        public async Task Register_Should_ReturnBadRequest_WhenEmailIsInvalid()
        {
            // Arrange
            var request = new RegisterUserRequest("not-an-email", "Password123!");

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/register", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Register_Should_ReturnBadRequest_WhenEmailIsAlreadyTaken()
        {
            // Arrange
            // Schritt 1: Erstelle einen Benutzer, damit die E-Mail bereits existiert.
            var initialRequest = new RegisterUserRequest("taken@example.com", "Password123!");
            await _client.PostAsJsonAsync("/api/authentication/register", initialRequest);

            // Schritt 2: Versuche, denselben Benutzer erneut zu registrieren.
            var duplicateRequest = new RegisterUserRequest("taken@example.com", "AnotherPassword456!");

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/register", duplicateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_Should_ReturnOkWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            // Erstelle zuerst einen Benutzer, um dich einloggen zu können.
            var userEmail = "login-user@example.com";
            var userPassword = "Password123!";
            var registerRequest = new RegisterUserRequest(userEmail, userPassword);
            await _client.PostAsJsonAsync("/api/authentication/register", registerRequest);

            var loginRequest = new LoginRequest(userEmail, userPassword);

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Assert.NotNull(loginResponse);
            Assert.False(string.IsNullOrEmpty(loginResponse.BasicToken));
        }

        [Fact]
        public async Task Login_Should_ReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var loginRequest = new LoginRequest("nonexistent@example.com", "password");

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Login_Should_ReturnNotFound_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userEmail = "wrong-password@example.com";
            var userPassword = "Password123!";
            var registerRequest = new RegisterUserRequest(userEmail, userPassword);
            await _client.PostAsJsonAsync("/api/authentication/register", registerRequest);

            var loginRequest = new LoginRequest(userEmail, "this-is-the-wrong-password");

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        // Fügen Sie diesen Test zu Ihrer AuthenticationControllerTests.cs hinzu

        [Fact]
        public async Task Login_Should_ReturnNotFound_WhenUserIsDeactivated()
        {
            // Arrange
            var userEmail = "deactivated-user@example.com";
            var userPassword = "Password123!";

            // 1. Benutzer registrieren
            var registerRequest = new RegisterUserRequest(userEmail, userPassword);
            var registerHttpResponse = await _client.PostAsJsonAsync("/api/authentication/register", registerRequest);
            var registerResponse = await registerHttpResponse.Content.ReadFromJsonAsync<RegisterUserResponse>();

            // 2. Benutzer direkt in der DB deaktivieren, um den Zustand zu simulieren
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                var userInDb = await dbContext.Users.FindAsync(registerResponse.UserId);
                userInDb.Deactivate();
                await dbContext.SaveChangesAsync(CancellationToken.None);
            }

            var loginRequest = new LoginRequest(userEmail, userPassword);

            // Act
            var response = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);

            // Assert
            // Der Handler sollte einen deaktivierten Benutzer genauso behandeln wie einen nicht gefundenen.
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
