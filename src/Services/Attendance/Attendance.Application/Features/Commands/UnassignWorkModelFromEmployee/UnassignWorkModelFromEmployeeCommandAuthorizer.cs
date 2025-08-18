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

namespace Attendance.Application.Features.Commands.UnassignWorkModelFromEmployee
{
    public class UnassignWorkModelFromEmployeeCommandAuthorizer : ICommandAuthorizer<UnassignWorkModelFromEmployeeCommand>
    {
        private readonly IEmployeeWorkModelRepository _employeeWorkModelRepository;
        private readonly IWorkModelRepository _workModelRepository;

        public UnassignWorkModelFromEmployeeCommandAuthorizer(
            IEmployeeWorkModelRepository employeeWorkModelRepository,
            IWorkModelRepository workModelRepository)
        {
            _employeeWorkModelRepository = employeeWorkModelRepository;
            _workModelRepository = workModelRepository;
        }

        public async Task AuthorizeAsync(UnassignWorkModelFromEmployeeCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var assignment = await _employeeWorkModelRepository.GetByIdAsync(command.EmployeeWorkModelId, cancellationToken);
            if (assignment is null)
            {
                throw new NotFoundException(nameof(EmployeeWorkModel), command.EmployeeWorkModelId);
            }

            var workModel = await _workModelRepository.GetByIdAsync(assignment.WorkModelId, cancellationToken);
            if (workModel is null)
            {
                // Sollte nicht passieren, wenn die Daten konsistent sind.
                throw new NotFoundException(nameof(WorkModel), assignment.WorkModelId);
            }

            var requiredPermission = WorkModelPermissions.Assign;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{workModel.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to unassign work models from this employee group is required.");
            }
        }
    }
}
