using Session.Contracts.Requests;
using Session.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.SessionApi
{
    public interface ISessionApiClient
    {
        Task<SessionTokensResponse> CreateSessionAsync(CreateSessionRequest request);
        Task<SessionTokensResponse> RefreshSessionAsync(RefreshTokenRequest request);
        Task RevokeSessionAsync(RevokeSessionRequest request);
        Task<List<ActiveSessionResponse>> GetMySessionsAsync();
    }
}
