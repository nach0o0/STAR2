using MediatR;
using Permission.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAssignmentsByScope
{
    public class GetAssignmentsByScopeQueryHandler : IRequestHandler<GetAssignmentsByScopeQuery, GetAssignmentsByScopeQueryResult>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IAuthServiceClient _authServiceClient;

        public GetAssignmentsByScopeQueryHandler(
            IUserPermissionAssignmentRepository assignmentRepository, 
            IRoleRepository roleRepository, 
            IPermissionRepository permissionRepository, 
            IAuthServiceClient authServiceClient)
        {
            _assignmentRepository = assignmentRepository; 
            _roleRepository = roleRepository; 
            _permissionRepository = permissionRepository; 
            _authServiceClient = authServiceClient;
        }

        public async Task<GetAssignmentsByScopeQueryResult> Handle(GetAssignmentsByScopeQuery request, CancellationToken ct)
        {
            var assignmentsInScope = await _assignmentRepository.GetAssignmentsByScopeAsync(request.Scope, ct);
            var resultList = new List<UserAssignmentsInScopeResult>();

            // Gruppiere alle Zuweisungen nach Benutzer-ID
            var assignmentsByUser = assignmentsInScope.GroupBy(a => a.UserId);

            foreach (var userGroup in assignmentsByUser)
            {
                var userId = userGroup.Key;
                var userDetails = await _authServiceClient.GetUserByIdAsync(userId, ct);
                var userEmail = userDetails?.Email ?? "Unknown User";

                var assignedRoleResults = new List<AssignedRoleInScopeResult>();
                var directPermissionResults = new List<DirectPermissionInScopeResult>();

                foreach (var assignment in userGroup)
                {
                    if (assignment.AssignmentType == Domain.Entities.AssignmentType.ROLE && assignment.RoleId.HasValue)
                    {
                        var role = await _roleRepository.GetByIdAsync(assignment.RoleId.Value, ct);
                        if (role != null)
                        {
                            var permissionsInRole = await _roleRepository.GetPermissionsForRoleAsync(role.Id, ct);
                            assignedRoleResults.Add(new AssignedRoleInScopeResult(role.Id, role.Name, permissionsInRole));
                        }
                    }
                    else if (assignment.AssignmentType == Domain.Entities.AssignmentType.PERMISSION && assignment.PermissionId != null)
                    {
                        var permission = await _permissionRepository.GetByIdAsync(assignment.PermissionId, ct);
                        if (permission != null)
                            directPermissionResults.Add(new DirectPermissionInScopeResult(permission.Id, permission.Description));
                    }
                }

                resultList.Add(new UserAssignmentsInScopeResult(userId, userEmail, assignedRoleResults, directPermissionResults));
            }

            return new GetAssignmentsByScopeQueryResult(resultList);
        }
    }
}
