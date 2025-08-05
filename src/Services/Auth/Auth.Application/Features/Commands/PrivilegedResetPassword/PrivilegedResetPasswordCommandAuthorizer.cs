using Auth.Domain.Authorization;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.PrivilegedResetPassword
{
    public class PrivilegedResetPasswordCommandAuthorizer : ICommandAuthorizer<PrivilegedResetPasswordCommand>
    {
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public PrivilegedResetPasswordCommandAuthorizer(IOrganizationServiceClient organizationServiceClient)
        {
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task AuthorizeAsync(PrivilegedResetPasswordCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // 1. Finde heraus, zu welcher Organisation der Ziel-Benutzer gehört.
            var employeeInfo = await _organizationServiceClient.GetEmployeeInfoByUserIdAsync(command.UserId, cancellationToken);

            var requiredPermission = UserPermissions.PrivilegedResetPassword;
            string requiredScope;

            // Prüft, ob der Mitarbeiter existiert UND einer Organisation zugewiesen ist.
            if (employeeInfo?.OrganizationId.HasValue == true)
            {
                // Fall 1: Der Benutzer gehört zu einer Organisation.
                requiredScope = $"{PermittedScopeTypes.Organization}:{employeeInfo.Value.OrganizationId.Value}";
            }
            else
            {
                // Fall 2: Der Benutzer hat kein Mitarbeiterprofil oder gehört zu keiner Organisation.
                requiredScope = PermittedScopeTypes.Global;
            }

            // 2. Prüfe, ob der ausführende Admin die Berechtigung im ermittelten Scope hat.
            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException($"Permission '{requiredPermission}' in scope '{requiredScope}' is required.");
            }
        }
    }
}
