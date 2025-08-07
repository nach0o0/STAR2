using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using WpfClient.Services.Application.Auth;

namespace WpfClient.Services.Api.Handlers
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthDelegatingHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var accessToken = authService.GetAccessToken();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
