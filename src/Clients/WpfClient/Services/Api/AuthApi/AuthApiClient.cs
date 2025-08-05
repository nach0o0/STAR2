using Auth.Contracts.Requests;
using Auth.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Exceptions;

namespace WpfClient.Services.Api.AuthApi
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/authentication/login", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/authentication/register", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<RegisterUserResponse>();
        }

        public async Task<MyProfileResponse> GetMyProfileAsync()
        {
            var httpResponse = await _httpClient.GetAsync("api/users/me");
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<MyProfileResponse>();
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/users/me/change-password", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task DeleteMyAccountAsync(DeleteMyAccountRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "api/users/me")
            {
                Content = JsonContent.Create(request)
            };
            var httpResponse = await _httpClient.SendAsync(httpRequest);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task PrivilegedResetPasswordAsync(Guid userId, PrivilegedResetPasswordRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync($"api/users/{userId}/privileged-reset-password", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }
    }
}
