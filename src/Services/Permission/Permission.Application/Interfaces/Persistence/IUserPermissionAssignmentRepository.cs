using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Interfaces.Persistence
{
    public interface IUserPermissionAssignmentRepository
    {
        Task<List<UserPermissionAssignment>> GetAssignmentsForUserAsync(Guid userId, IEnumerable<string> scopes, CancellationToken cancellationToken = default);
        Task<List<UserPermissionAssignment>> GetAssignmentsForUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserPermissionAssignment?> FindDirectPermissionAssignmentAsync(Guid userId, string permissionId, string scope, CancellationToken cancellationToken = default);
        Task<UserPermissionAssignment?> FindRoleAssignmentAsync(Guid userId, Guid roleId, string scope, CancellationToken cancellationToken = default);
        Task<List<string>> GetScopesForRoleAssignmentAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<int> CountUsersWithPermissionInScopeAsync(string permissionId, string scope, CancellationToken cancellationToken = default);
        Task<bool> IsUserTheLastAdminForPermissionAsync(Guid userId, string permissionId, string scope, CancellationToken cancellationToken);
        Task AddAsync(UserPermissionAssignment assignment, CancellationToken cancellationToken = default);
        void Delete(UserPermissionAssignment assignment);
        Task<List<UserPermissionAssignment>> GetDirectAssignmentsByPermissionIdAsync(string permissionId, CancellationToken cancellationToken = default);
        Task<List<UserPermissionAssignment>> GetAssignmentsForRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<List<Guid>> GetUserIdsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<List<UserPermissionAssignment>> GetAssignmentsByScopeAsync(string scope, CancellationToken cancellationToken = default);
    }
}
