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

namespace CostObject.Application.Features.Commands.RejectCostObjectRequest
{
    public class RejectCostObjectRequestCommandAuthorizer : ICommandAuthorizer<RejectCostObjectRequestCommand>
    {
        private readonly ICostObjectRequestRepository _requestRepository;

        public RejectCostObjectRequestCommandAuthorizer(ICostObjectRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task AuthorizeAsync(RejectCostObjectRequestCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var request = await _requestRepository.GetByIdAsync(command.CostObjectRequestId, cancellationToken);
            if (request is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObjectRequest), command.CostObjectRequestId);
            }

            var requiredPermission = CostObjectPermissions.Approve; // Dieselbe Berechtigung wie für die Genehmigung
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{request.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to reject cost object requests in this employee group is required.");
            }
        }
    }
}
