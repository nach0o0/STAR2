using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClient.Exceptions;

namespace WpfClient.Services.Api
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task EnsureSuccessOrThrowApiException(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Dictionary<string, string[]>? validationErrors = null;

                // Versuche, die Fehlerdetails zu deserialisieren
                try
                {
                    var errorDto = JsonSerializer.Deserialize<JsonElement>(errorContent);
                    if (errorDto.TryGetProperty("errors", out var errorsElement))
                    {
                        validationErrors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(errorsElement.GetRawText());
                    }
                }
                catch { /* Ignorieren, wenn es kein JSON ist */ }

                throw new ApiException(errorContent, (int)response.StatusCode, validationErrors);
            }
        }
    }
}
