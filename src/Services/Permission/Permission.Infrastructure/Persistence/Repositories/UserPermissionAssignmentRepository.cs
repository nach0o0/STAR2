using Microsoft.EntityFrameworkCore;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.Persistence.Repositories
{
    public class UserPermissionAssignmentRepository : IUserPermissionAssignmentRepository
    {
        private readonly PermissionDbContext _dbContext;
        private readonly IRoleRepository _roleRepository;

        public UserPermissionAssignmentRepository(PermissionDbContext dbContext, IRoleRepository roleRepository)
        {
            _dbContext = dbContext;
            _roleRepository = roleRepository;
        }

        public async Task AddAsync(UserPermissionAssignment assignment, CancellationToken cancellationToken = default)
        {
            await _dbContext.UserPermissionAssignments.AddAsync(assignment, cancellationToken);
        }

        public void Delete(UserPermissionAssignment assignment)
        {
            _dbContext.UserPermissionAssignments.Remove(assignment);
        }

        public async Task<UserPermissionAssignment?> FindDirectPermissionAssignmentAsync(Guid userId, string permissionId, string scope, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .FirstOrDefaultAsync(a =>
                    a.UserId == userId &&
                    a.PermissionId == permissionId &&
                    a.Scope == scope &&
                    a.AssignmentType == AssignmentType.PERMISSION,
                    cancellationToken);
        }

        public async Task<UserPermissionAssignment?> FindRoleAssignmentAsync(Guid userId, Guid roleId, string scope, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .FirstOrDefaultAsync(a => a.UserId == userId && a.RoleId == roleId && a.Scope == scope, cancellationToken);
        }

        public async Task<List<string>> GetScopesForRoleAssignmentAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.RoleId == roleId)
                .Select(a => a.Scope)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountUsersWithPermissionInScopeAsync(string permissionId, string scope, CancellationToken cancellationToken = default)
        {
            var rolesWithPermission = await _roleRepository.GetRolesContainingPermissionAsync(permissionId, scope, cancellationToken);

            var userIds = await _dbContext.UserPermissionAssignments
                .Where(a => a.Scope == scope &&
                            (a.PermissionId == permissionId || (a.RoleId.HasValue && rolesWithPermission.Contains(a.RoleId.Value))))
                .Select(a => a.UserId)
                .Distinct()
                .ToListAsync(cancellationToken);

            return userIds.Count;
        }

        public async Task<bool> IsUserTheLastAdminForPermissionAsync(Guid userId, string permissionId, string scope, CancellationToken cancellationToken)
        {
            var adminCount = await CountUsersWithPermissionInScopeAsync(permissionId, scope, cancellationToken);
            if (adminCount > 1) return false;

            var userAssignments = await GetAssignmentsForUserAsync(userId, new[] { scope }, cancellationToken);
            var rolesWithPermission = await _roleRepository.GetRolesContainingPermissionAsync(permissionId, scope, cancellationToken);

            return userAssignments.Any(a => a.PermissionId == permissionId || (a.RoleId.HasValue && rolesWithPermission.Contains(a.RoleId.Value)));
        }

        public async Task<List<UserPermissionAssignment>> GetAssignmentsForUserAsync(Guid userId, IEnumerable<string> scopes, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.UserId == userId && scopes.Contains(a.Scope))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserPermissionAssignment>> GetAssignmentsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserPermissionAssignment>> GetDirectAssignmentsByPermissionIdAsync(string permissionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.PermissionId == permissionId && a.AssignmentType == AssignmentType.PERMISSION)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserPermissionAssignment>> GetAssignmentsForRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.RoleId == roleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Guid>> GetUserIdsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.RoleId == roleId)
                .Select(a => a.UserId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserPermissionAssignment>> GetAssignmentsByScopeAsync(string scope, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.Scope == scope)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetScopesForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.UserId == userId)
                .Select(a => a.Scope)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
