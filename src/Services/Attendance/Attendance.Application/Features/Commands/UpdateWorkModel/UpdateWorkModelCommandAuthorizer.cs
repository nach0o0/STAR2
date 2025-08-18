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

namespace Attendance.Application.Features.Commands.UpdateWorkModel
{
    public class UpdateWorkModelCommandAuthorizer : ICommandAuthorizer<UpdateWorkModelCommand>
    {
        private readonly IWorkModelRepository _workModelRepository;

        public UpdateWorkModelCommandAuthorizer(IWorkModelRepository workModelRepository)
        {
            _workModelRepository = workModelRepository;
        }

        public async Task AuthorizeAsync(UpdateWorkModelCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var workModel = await _workModelRepository.GetByIdAsync(command.WorkModelId, cancellationToken);
            if (workModel is null)
            {
                throw new NotFoundException(nameof(WorkModel), command.WorkModelId);
            }

            var requiredPermission = WorkModelPermissions.Update;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{workModel.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to update work models in this employee group is required.");
            }
        }
    }
}
