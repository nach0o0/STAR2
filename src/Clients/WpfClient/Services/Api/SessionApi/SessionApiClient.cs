using Session.Contracts.Requests;
using Session.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Exceptions;

namespace WpfClient.Services.Api.SessionApi
{
    public class SessionApiClient : ISessionApiClient
    {
        private readonly HttpClient _httpClient;

        public SessionApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SessionTokensResponse> CreateSessionAsync(CreateSessionRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/sessions", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<SessionTokensResponse>();
        }

        public async Task<SessionTokensResponse> RefreshSessionAsync(RefreshTokenRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/sessions/refresh", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<SessionTokensResponse>();
        }

        public async Task RevokeSessionAsync(RevokeSessionRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "api/sessions")
            {
                Content = JsonContent.Create(request)
            };
            var httpResponse = await _httpClient.SendAsync(httpRequest);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task<List<ActiveSessionResponse>> GetMySessionsAsync()
        {
            var httpResponse = await _httpClient.GetAsync("api/sessions/me");
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<List<ActiveSessionResponse>>();
        }
    }
}
