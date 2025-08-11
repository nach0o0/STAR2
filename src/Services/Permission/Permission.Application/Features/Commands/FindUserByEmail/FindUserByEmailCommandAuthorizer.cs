using Permission.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.FindUserByEmail
{
    public class FindUserByEmailCommandAuthorizer : ICommandAuthorizer<FindUserByEmailCommand>
    {
        public Task AuthorizeAsync(FindUserByEmailCommand command, CurrentUser currentUser, CancellationToken ct)
        {
            // Definiere die Liste der Berechtigungen, von denen mindestens eine erforderlich ist.
            var requiredPermissions = new[]
            {
                AssignmentPermissions.AssignRole,
                AssignmentPermissions.AssignDirect
            };

            var requiredScope = command.Scope;

            // Prüfe, ob der Benutzer die Berechtigungen für den angeforderten Scope hat.
            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var userPermissionsInScope))
            {
                // Wenn der Benutzer gar keine Berechtigungen in diesem Scope hat, verweigere den Zugriff.
                throw new ForbiddenAccessException("You are not authorized to find and add users to this scope.");
            }

            // Prüfe, ob es eine Schnittmenge zwischen den benötigten und den vorhandenen Berechtigungen gibt.
            // .Any() gibt true zurück, sobald die erste Übereinstimmung gefunden wird.
            var hasRequiredPermission = requiredPermissions.Any(required => userPermissionsInScope.Contains(required));

            if (!hasRequiredPermission)
            {
                throw new ForbiddenAccessException("You are not authorized to find and add users to this scope.");
            }

            return Task.CompletedTask;
        }
    }
}
