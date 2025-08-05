using MediatR;
using Microsoft.Extensions.Options;
using Session.Application.Interfaces.Infrastructure;
using Session.Application.Interfaces.Persistence;
using Session.Domain.Entities;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Domain.Exceptions;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (string AccessToken, string RefreshToken)>
    {
        private readonly IActiveSessionRepository _sessionRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEnrichedTokenService _enrichedTokenService;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly JwtOptions _jwtOptions;

        public RefreshTokenCommandHandler(
            IActiveSessionRepository sessionRepository,
            IPasswordHasher passwordHasher,
            IEnrichedTokenService enrichedTokenService,
            IRefreshTokenGenerator refreshTokenGenerator,
            IOptions<JwtOptions> jwtOptions)
        {
            _sessionRepository = sessionRepository;
            _passwordHasher = passwordHasher;
            _enrichedTokenService = enrichedTokenService;
            _refreshTokenGenerator = refreshTokenGenerator;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<(string AccessToken, string RefreshToken)> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            // 1. Teile das vom Client empfangene Token
            var parts = command.RefreshToken.Split(':');
            if (parts.Length != 2)
            {
                throw new ForbiddenAccessException("Invalid refresh token format.");
            }
            var selector = parts[0];
            var verifier = parts[1];

            // 2. Führe eine schnelle, indizierte Suche nach dem Selector durch
            var session = await _sessionRepository.GetBySelectorAsync(selector, cancellationToken);
            if (session is null || session.IsExpired)
            {
                throw new ForbiddenAccessException("Invalid or expired refresh token.");
            }

            // 3. Führe einen sicheren BCrypt-Vergleich mit dem Verifier durch
            var isVerifierValid = _passwordHasher.VerifyPassword(verifier, session.VerifierHash);
            if (!isVerifierValid)
            {
                throw new ForbiddenAccessException("Invalid or expired refresh token.");
            }

            // Erstelle das neue Access Token direkt aus der UserId der gefundenen Session.
            var newAccessToken = await _enrichedTokenService.GenerateEnrichedAccessTokenAsync(session.UserId, cancellationToken);

            // Token Rotation: Erstelle ein neues Selector/Verifier-Paar.
            var (newSelector, newVerifier) = _refreshTokenGenerator.Generate();
            var newVerifierHash = _passwordHasher.HashPassword(newVerifier);

            // Aktualisiere die bestehende Session mit den neuen Werten.
            session.RotateRefreshToken(
                newSelector,
                newVerifierHash,
                _jwtOptions.RefreshTokenExpirationDays);

            var newRefreshToken = $"{newSelector}:{newVerifier}";

            return (newAccessToken, newRefreshToken);
        }
    }
}
