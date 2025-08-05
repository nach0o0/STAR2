using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AspNetCore.Middleware
{
    public class GatewayIpCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GatewayIpCheckMiddleware> _logger;

        public GatewayIpCheckMiddleware(RequestDelegate next, ILogger<GatewayIpCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;

            // Prüfe, ob die IP-Adresse eine Loopback-Adresse ist (localhost, 127.0.0.1, ::1)
            if (remoteIp is null || !IPAddress.IsLoopback(remoteIp))
            {
                _logger.LogWarning($"Forbidden request from non-localhost IP: {remoteIp}");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return; // Beende die Anfrage hier
            }

            // Wenn die IP-Adresse localhost ist, fahre mit der normalen Pipeline fort.
            await _next(context);
        }
    }
}
