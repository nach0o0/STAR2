using Organization.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByOrganization
{
    public class GetEmployeesByOrganizationQueryAuthorizer : ICommandAuthorizer<GetEmployeesByOrganizationQuery>
    {
        public Task AuthorizeAsync(GetEmployeesByOrganizationQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Prüfung 1: Ist der Benutzer ein Mitarbeiter DIESER Organisation?
            if (currentUser.OrganizationId == command.OrganizationId)
            {
                // Ja, Autorisierung erfolgreich.
                return Task.CompletedTask;
            }

            // Prüfung 2: Wenn nicht, hat der Benutzer die explizite Berechtigung?
            var requiredPermission = EmployeePermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.OrganizationId}";

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                // Ja, Autorisierung erfolgreich.
                return Task.CompletedTask;
            }

            // Wenn keine der beiden Prüfungen erfolgreich war, Zugriff verweigern.
            throw new ForbiddenAccessException("You are not authorized to view employees of this organization.");
        }
    }
}
