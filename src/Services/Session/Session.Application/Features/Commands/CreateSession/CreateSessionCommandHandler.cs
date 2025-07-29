using MediatR;
using Microsoft.Extensions.Options;
using Session.Application.Interfaces.Infrastructure;
using Session.Application.Interfaces.Persistence;
using Session.Domain.Entities;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Commands.CreateSession
{
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, (string AccessToken, string RefreshToken)>
    {
        private readonly IEnrichedTokenService _enrichedTokenService;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IActiveSessionRepository _sessionRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtOptions _jwtOptions;

        public CreateSessionCommandHandler(
            IEnrichedTokenService enrichedTokenService,
            IRefreshTokenGenerator refreshTokenGenerator,
            IActiveSessionRepository sessionRepository,
            IPasswordHasher passwordHasher,
            IOptions<JwtOptions> jwtOptions)
        {
            _enrichedTokenService = enrichedTokenService;
            _refreshTokenGenerator = refreshTokenGenerator;
            _sessionRepository = sessionRepository;
            _passwordHasher = passwordHasher;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<(string AccessToken, string RefreshToken)> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
        {
            // 1. Neues, angereichertes Access Token generieren lassen.
            //    Dieser Service validiert das Basis-Token und holt die Claims.
            var accessToken = await _enrichedTokenService.GenerateEnrichedAccessTokenAsync(command.BasicToken, cancellationToken);

            // 2. Refresh Token generieren und für die Speicherung hashen.
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();
            var refreshTokenHash = _passwordHasher.HashPassword(refreshToken);

            // 3. UserId aus dem (bereits validierten) Basis-Token extrahieren.
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ReadJwtToken(command.BasicToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                // Sollte nie passieren, wenn das Token vom Auth-Service korrekt ausgestellt wurde.
                throw new InvalidOperationException("Invalid basic token provided.");
            }

            // 4. Neue ActiveSession-Entität erstellen.
            var session = new ActiveSession(
                userId,
                refreshTokenHash,
                _jwtOptions.RefreshTokenExpirationDays,
                command.ClientInfo);

            await _sessionRepository.AddAsync(session, cancellationToken);

            // 5. Die finalen Tokens zurückgeben.
            return (accessToken, refreshToken);
        }
    }
}
