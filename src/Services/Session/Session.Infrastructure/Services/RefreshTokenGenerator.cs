using Session.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Session.Infrastructure.Services
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public (string Selector, string Verifier) Generate()
        {
            var selectorBytes = new byte[16];
            var verifierBytes = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(selectorBytes);
            rng.GetBytes(verifierBytes);

            // Verwende eine URL-sichere Base64-Kodierung
            var selector = Convert.ToBase64String(selectorBytes)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var verifier = Convert.ToBase64String(verifierBytes)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            return (selector, verifier);
        }
    }
}
