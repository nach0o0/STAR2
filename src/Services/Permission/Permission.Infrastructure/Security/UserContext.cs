using Microsoft.AspNetCore.Http;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using System.Security.Claims;
using System.Text.Json;

namespace Permission.Infrastructure.Security
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            // Versuche, die Claims zu parsen. Wenn sie nicht da sind, bleibt der Wert null.
            _ = Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

            Guid? employeeId = Guid.TryParse(user.FindFirstValue(CustomClaimTypes.EmployeeId), out var parsedEmployeeId)
                ? parsedEmployeeId
                : null;

            Guid? organizationId = Guid.TryParse(user.FindFirstValue(CustomClaimTypes.OrganizationId), out var parsedOrgId)
                ? parsedOrgId
                : null;

            var employeeGroupIds = user.FindAll(CustomClaimTypes.EmployeeGroupId)
                                   .Select(c => Guid.TryParse(c.Value, out var id) ? id : Guid.Empty)
                                   .Where(id => id != Guid.Empty)
                                   .ToList();

            var permissionsClaim = user.FindFirstValue(CustomClaimTypes.PermissionsByScope);
            var permissionsByScope = new Dictionary<string, List<string>>();
            if (!string.IsNullOrEmpty(permissionsClaim))
            {
                permissionsByScope = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(permissionsClaim)
                                     ?? new Dictionary<string, List<string>>();
            }

            return new CurrentUser(userId, employeeId, organizationId, employeeGroupIds, permissionsByScope);
        }
    }
}
