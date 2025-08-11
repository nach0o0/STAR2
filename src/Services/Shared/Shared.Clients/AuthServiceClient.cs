using Auth.Contracts.Responses;
using Shared.Application.Interfaces.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Clients
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private readonly HttpClient _httpClient;

        public AuthServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(Guid UserId, string Email, string? FirstName, string? LastName)?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var responseDto = await _httpClient.GetFromJsonAsync<UserDetailsResponse>($"api/internal/users/by-email?email={email}", cancellationToken);
            if (responseDto is null)
            {
                return null;
            }
            return (responseDto.UserId, responseDto.Email, responseDto.FirstName, responseDto.LastName);
        }

        public async Task<(Guid UserId, string Email)?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var responseDto = await _httpClient.GetFromJsonAsync<UserResponse>($"api/internal/users/{userId}", cancellationToken);
                return responseDto is null ? null : (responseDto.UserId, responseDto.Email);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
