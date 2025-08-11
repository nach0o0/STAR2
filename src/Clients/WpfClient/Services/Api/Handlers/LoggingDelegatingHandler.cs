using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.Handlers
{
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("==============================================================================");
            Debug.WriteLine($"[API-REQUEST] Sending request to: {request.Method} {request.RequestUri}");

            // Protokolliere den Request-Body, falls vorhanden
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
                Debug.WriteLine($"[API-REQUEST] Body: {requestBody}");
            }

            // Sende die Anfrage an den nächsten Handler in der Kette
            var response = await base.SendAsync(request, cancellationToken);

            Debug.WriteLine($"[API-RESPONSE] Received response with status code: {response.StatusCode}");

            // Protokolliere den Response-Body, falls vorhanden
            if (response.Content != null)
            {
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                Debug.WriteLine($"[API-RESPONSE] Body: {responseBody}");
            }
            Debug.WriteLine("==============================================================================");

            return response;
        }
    }
}
