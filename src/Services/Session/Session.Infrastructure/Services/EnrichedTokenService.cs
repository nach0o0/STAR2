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

        public EnrichedTokenService(
            IOptions<JwtOptions> jwtOptions,
            IOrganizationServiceClient orgServiceClient,
            IPermissionQueryClient permissionServiceClient)
        {
            _jwtOptions = jwtOptions.Value;
            _orgServiceClient = orgServiceClient;
            _permissionServiceClient = permissionServiceClient;
        }

        public async Task<string> GenerateEnrichedAccessTokenAsync(string basicToken, CancellationToken cancellationToken = default)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

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

            var claims = principal.Claims.ToList();
            var userId = Guid.Parse(claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);

            var employeeInfo = await _orgServiceClient.GetEmployeeInfoByUserIdAsync(userId, cancellationToken);
            var scopes = new List<string>();

            if (employeeInfo is not null)
            {
                claims.Add(new Claim(CustomClaimTypes.EmployeeId, employeeInfo.Value.EmployeeId.ToString()));
                claims.Add(new Claim(CustomClaimTypes.OrganizationId, employeeInfo.Value.OrganizationId.ToString()));
                scopes.Add($"{PermittedScopeTypes.Organization}:{employeeInfo.Value.OrganizationId}");

                foreach (var groupId in employeeInfo.Value.EmployeeGroupIds)
                {
                    claims.Add(new Claim(CustomClaimTypes.EmployeeGroupId, groupId.ToString()));
                    scopes.Add($"{PermittedScopeTypes.EmployeeGroup}:{groupId}");
                }
            }

            scopes.Add(PermittedScopeTypes.Global);
            if (scopes.Any())
            {
                var permissionsByScope = await _permissionServiceClient.GetPermissionsForUserAsync(userId, scopes, cancellationToken);
                if (permissionsByScope is not null && permissionsByScope.Any())
                {
                    var permissionsJson = JsonSerializer.Serialize(permissionsByScope);
                    claims.Add(new Claim(CustomClaimTypes.PermissionsByScope, permissionsJson, JsonClaimValueTypes.Json));
                }
            }

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
