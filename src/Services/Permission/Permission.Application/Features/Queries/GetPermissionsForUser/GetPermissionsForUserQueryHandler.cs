using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsForUser
{
    public class GetPermissionsForUserQueryHandler
        : IRequestHandler<GetPermissionsForUserQuery, Dictionary<string, List<string>>>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IRoleRepository _roleRepository;

        public GetPermissionsForUserQueryHandler(
            IUserPermissionAssignmentRepository assignmentRepository,
            IRoleRepository roleRepository)
        {
            _assignmentRepository = assignmentRepository;
            _roleRepository = roleRepository;
        }

        public async Task<Dictionary<string, List<string>>> Handle(
            GetPermissionsForUserQuery query,
            CancellationToken cancellationToken)
        {
            var assignments = await _assignmentRepository
                .GetAssignmentsForUserAsync(query.UserId, query.Scopes, cancellationToken);

            if (!assignments.Any())
            {
                return new Dictionary<string, List<string>>();
            }

            var permissionsByScope = new Dictionary<string, List<string>>();

            // Direkte Berechtigungen hinzufügen
            foreach (var assignment in assignments.Where(a => a.AssignmentType == AssignmentType.PERMISSION))
            {
                if (!permissionsByScope.ContainsKey(assignment.Scope))
                {
                    permissionsByScope[assignment.Scope] = new List<string>();
                }
                permissionsByScope[assignment.Scope].Add(assignment.PermissionId!);
            }

            // Rollen-Berechtigungen hinzufügen
            var rolesByScope = assignments
                .Where(a => a.AssignmentType == AssignmentType.ROLE)
                .GroupBy(a => a.Scope)
                .ToDictionary(g => g.Key, g => g.Select(a => a.RoleId!.Value).ToList());

            foreach (var scope in rolesByScope.Keys)
            {
                var roleIds = rolesByScope[scope];
                var rolePermissions = await _roleRepository.GetPermissionsForRolesAsync(roleIds, cancellationToken);

                if (!permissionsByScope.ContainsKey(scope))
                {
                    permissionsByScope[scope] = new List<string>();
                }
                permissionsByScope[scope].AddRange(rolePermissions);
            }

            // Eindeutige Berechtigungen pro Scope sicherstellen
            return permissionsByScope.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Distinct().ToList());
        }
    }
}
