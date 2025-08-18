using Auth.Api;
using Auth.Application.IntegrationTests.TestUtils;
using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory<AuthApiMarker>>
    {
        protected readonly CustomWebApplicationFactory<AuthApiMarker> _factory;
        protected readonly HttpClient _client;

        protected BaseIntegrationTest(CustomWebApplicationFactory<AuthApiMarker> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        protected async Task<HttpClient> CreateAuthenticatedClientAsync(string email = "testuser@example.com", string password = "Password123!")
        {
            // Registrieren
            var registerRequest = new RegisterUserRequest(email, password);
            await _client.PostAsJsonAsync("/api/authentication/register", registerRequest);

            // Anmelden
            var loginRequest = new LoginRequest(email, password);
            var loginResponse = await _client.PostAsJsonAsync("/api/authentication/login", loginRequest);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            // Authentifizierten Client erstellen
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.BasicToken);

            return client;
        }
    }
}
