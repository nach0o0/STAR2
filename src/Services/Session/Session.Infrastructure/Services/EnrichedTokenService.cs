using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Session.Application.Interfaces.Infrastructure;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Session.Infrastructure.Services
{
    public class EnrichedTokenService : IEnrichedTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IOrganizationServiceClient _orgServiceClient;
        private readonly IPermissionQueryClient _permissionServiceClient;
        private readonly IAuthServiceClient _authServiceClient;

        public EnrichedTokenService(
            IOptions<JwtOptions> jwtOptions,
            IOrganizationServiceClient orgServiceClient,
            IPermissionQueryClient permissionServiceClient,
            IAuthServiceClient authServiceClient)
        {
            _jwtOptions = jwtOptions.Value;
            _orgServiceClient = orgServiceClient;
            _permissionServiceClient = permissionServiceClient;
            _authServiceClient = authServiceClient;
        }

        // Methode für den initialen Login (nimmt das Basis-Token)
        public async Task<string> GenerateEnrichedAccessTokenAsync(string basicToken, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            // Validiere das Basis-Token, um die UserId sicher zu extrahieren.
            var principal = tokenHandler.ValidateToken(basicToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Rufe die zentrale Methode mit der extrahierten UserId auf.
            return await GenerateEnrichedAccessTokenAsync(userId, cancellationToken);
        }

        // Zentrale Methode für die Anreicherung (nimmt die UserId)
        public async Task<string> GenerateEnrichedAccessTokenAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _authServiceClient.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            // 1. Erstelle die Basis-Claims für das neue Token.
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Value.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 2. Anreicherung mit Organisations-Daten
            var employeeInfo = await _orgServiceClient.GetEmployeeInfoByUserIdAsync(userId, cancellationToken);
            var scopes = new List<string>();

            if (employeeInfo is not null)
            {
                claims.Add(new Claim(CustomClaimTypes.EmployeeId, employeeInfo.Value.EmployeeId.ToString()));
                if (employeeInfo.Value.OrganizationId.HasValue)
                {
                    claims.Add(new Claim(CustomClaimTypes.OrganizationId, employeeInfo.Value.OrganizationId.Value.ToString()));
                    scopes.Add($"{PermittedScopeTypes.Organization}:{employeeInfo.Value.OrganizationId.Value}");
                }

                foreach (var groupId in employeeInfo.Value.EmployeeGroupIds)
                {
                    claims.Add(new Claim(CustomClaimTypes.EmployeeGroupId, groupId.ToString()));
                    scopes.Add($"{PermittedScopeTypes.EmployeeGroup}:{groupId}");
                }
            }

            // 3. Anreicherung mit Berechtigungs-Daten
            scopes.Add(PermittedScopeTypes.Global); // Füge immer den globalen Scope hinzu.
            var permissionsByScope = await _permissionServiceClient.GetPermissionsForUserAsync(userId, scopes, cancellationToken);
            if (permissionsByScope is not null && permissionsByScope.Any())
            {
                var permissionsJson = JsonSerializer.Serialize(permissionsByScope);
                claims.Add(new Claim(CustomClaimTypes.PermissionsByScope, permissionsJson, JsonClaimValueTypes.Json));
            }

            // 4. Finales Token erstellen
            var enrichedToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return tokenHandler.WriteToken(enrichedToken);
        }
    }
}
