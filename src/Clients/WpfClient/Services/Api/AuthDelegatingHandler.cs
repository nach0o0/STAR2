using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Application.Auth;

namespace WpfClient.Services.Api
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        public AuthDelegatingHandler(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _accessTokenProvider.GetAccessToken();

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Füge den 'Authorization'-Header zur Anfrage hinzu.
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
