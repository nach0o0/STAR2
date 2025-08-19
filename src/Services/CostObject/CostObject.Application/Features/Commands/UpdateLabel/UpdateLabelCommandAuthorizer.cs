using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateLabel
{
    public class UpdateLabelCommandAuthorizer : ICommandAuthorizer<UpdateLabelCommand>
    {
        private readonly ILabelRepository _labelRepository;

        public UpdateLabelCommandAuthorizer(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task AuthorizeAsync(UpdateLabelCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var label = await _labelRepository.GetByIdAsync(command.LabelId, cancellationToken);
            if (label is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Label), command.LabelId);
            }

            var requiredPermission = LabelPermissions.Update;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{label.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to update labels in this employee group is required.");
            }
        }
    }
}
