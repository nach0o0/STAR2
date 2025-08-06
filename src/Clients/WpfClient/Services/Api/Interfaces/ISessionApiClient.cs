using Refit;
using Session.Contracts.Requests;
using Session.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.Interfaces
{
    public interface ISessionApiClient
    {
        [Post("/api/sessions")]
        Task<SessionTokensResponse> CreateSessionAsync([Body] CreateSessionRequest request);

        [Post("/api/sessions/refresh")]
        Task<SessionTokensResponse> RefreshSessionAsync([Body] RefreshTokenRequest request);

        [Delete("/api/sessions")]
        Task RevokeSessionAsync([Body] RevokeSessionRequest request);

        [Get("/api/sessions/me")]
        Task<List<ActiveSessionResponse>> GetMySessionsAsync();
    }
}
