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

namespace Attendance.Application.Features.Commands.UpdateEmployeeWorkModelAssignment
{
    public class UpdateEmployeeWorkModelAssignmentCommandAuthorizer : ICommandAuthorizer<UpdateEmployeeWorkModelAssignmentCommand>
    {
        private readonly IEmployeeWorkModelRepository _assignmentRepository;
        private readonly IWorkModelRepository _workModelRepository;

        public UpdateEmployeeWorkModelAssignmentCommandAuthorizer(
            IEmployeeWorkModelRepository assignmentRepository,
            IWorkModelRepository workModelRepository)
        {
            _assignmentRepository = assignmentRepository;
            _workModelRepository = workModelRepository;
        }

        public async Task AuthorizeAsync(UpdateEmployeeWorkModelAssignmentCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(command.AssignmentId, cancellationToken);
            if (assignment is null)
            {
                throw new NotFoundException(nameof(EmployeeWorkModel), command.AssignmentId);
            }

            var workModel = await _workModelRepository.GetByIdAsync(assignment.WorkModelId, cancellationToken);
            if (workModel is null)
            {
                throw new NotFoundException(nameof(WorkModel), assignment.WorkModelId);
            }

            var requiredPermission = WorkModelPermissions.Assign;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{workModel.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to update work model assignments in this employee group is required.");
            }
        }
    }
}
