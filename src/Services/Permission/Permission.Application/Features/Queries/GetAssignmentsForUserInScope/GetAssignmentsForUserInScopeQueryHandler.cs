using MediatR;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAssignmentsForUserInScope
{
    public class GetAssignmentsForUserInScopeQueryHandler : IRequestHandler<GetAssignmentsForUserInScopeQuery, GetAssignmentsForUserInScopeQueryResult>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public GetAssignmentsForUserInScopeQueryHandler(
            IUserPermissionAssignmentRepository assignmentRepository, 
            IRoleRepository roleRepository, 
            IPermissionRepository permissionRepository)
        {
            _assignmentRepository = assignmentRepository; 
            _roleRepository = roleRepository; 
            _permissionRepository = permissionRepository;
        }

        public async Task<GetAssignmentsForUserInScopeQueryResult> Handle(GetAssignmentsForUserInScopeQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _assignmentRepository.GetAssignmentsForUserAsync(request.UserId, new[] { request.Scope }, cancellationToken);

            var assignedRoleResults = new List<AssignedRoleResult>();
            var directPermissionResults = new List<DirectPermissionResult>();

            foreach (var assignment in assignments)
            {
                if (assignment.AssignmentType == Domain.Entities.AssignmentType.ROLE && assignment.RoleId.HasValue)
                {
                    var role = await _roleRepository.GetByIdAsync(assignment.RoleId.Value, cancellationToken);
                    if (role != null)
                    {
                        var permissionsInRole = await _roleRepository.GetPermissionsForRoleAsync(role.Id, cancellationToken);
                        assignedRoleResults.Add(new AssignedRoleResult(role.Id, role.Name, permissionsInRole));
                    }
                }
                else if (assignment.AssignmentType == Domain.Entities.AssignmentType.PERMISSION && assignment.PermissionId != null)
                {
                    var permission = await _permissionRepository.GetByIdAsync(assignment.PermissionId, cancellationToken);
                    if (permission != null)
                    {
                        directPermissionResults.Add(new DirectPermissionResult(permission.Id, permission.Description));
                    }
                }
            }
            return new GetAssignmentsForUserInScopeQueryResult(assignedRoleResults, directPermissionResults);
        }
    }
}
