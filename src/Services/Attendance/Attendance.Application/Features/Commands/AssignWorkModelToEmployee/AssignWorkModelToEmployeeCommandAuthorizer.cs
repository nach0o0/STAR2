using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Authorization;
using Attendance.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.AssignWorkModelToEmployee
{
    public class AssignWorkModelToEmployeeCommandAuthorizer : ICommandAuthorizer<AssignWorkModelToEmployeeCommand>
    {
        private readonly IWorkModelRepository _workModelRepository;

        public AssignWorkModelToEmployeeCommandAuthorizer(IWorkModelRepository workModelRepository)
        {
            _workModelRepository = workModelRepository;
        }

        public async Task AuthorizeAsync(AssignWorkModelToEmployeeCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // 1. Lade das Arbeitsmodell, um dessen EmployeeGroupId zu bekommen.
            var workModel = await _workModelRepository.GetByIdAsync(command.WorkModelId, cancellationToken);
            if (workModel is null)
            {
                throw new NotFoundException(nameof(WorkModel), command.WorkModelId);
            }

            // 2. Definiere die benötigte Berechtigung und den Scope.
            var requiredPermission = WorkModelPermissions.Assign;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{workModel.EmployeeGroupId}";

            // 3. Prüfe, ob der Benutzer die Berechtigung in diesem Scope hat.
            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to assign work models from this employee group is required.");
            }
        }
    }
}
