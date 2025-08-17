using Microsoft.Extensions.DependencyInjection;
using System.Net;
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

            // Sende die ursprüngliche Anfrage
            var response = await base.SendAsync(request, cancellationToken);

            // --- NEUE, INTELLIGENTE LOGIK ---
            // Prüfe, ob die Anfrage fehlgeschlagen ist, weil das Token abgelaufen ist.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                bool refreshedSuccessfully = false;
                await using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                    // Versuche, eine neue Session (und damit ein neues Access Token) zu bekommen.
                    refreshedSuccessfully = await authService.TryInitializeSessionAsync();
                }

                // Wenn die Erneuerung erfolgreich war...
                if (refreshedSuccessfully)
                {
                    // ... wiederhole die ursprünglich fehlgeschlagene Anfrage.
                    // Sie wird jetzt das neue, gültige Access Token verwenden.
                    await using (var scope = _serviceProvider.CreateAsyncScope())
                    {
                        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                        var newAccessToken = authService.GetAccessToken();
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                    }

                    // Sende die Anfrage erneut.
                    response = await base.SendAsync(request, cancellationToken);
                }
            }
            // ---------------------------------

            return response;
        }
    }
}
