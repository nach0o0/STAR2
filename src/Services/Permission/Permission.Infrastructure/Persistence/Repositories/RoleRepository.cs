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
    public class RoleRepository : IRoleRepository
    {
        private readonly PermissionDbContext _dbContext;

        public RoleRepository(PermissionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
        {
            await _dbContext.Roles.AddAsync(role, cancellationToken);
        }

        public void Delete(Role role)
        {
            _dbContext.Roles.Remove(role);
        }

        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<string>> GetPermissionsForRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetPermissionsForRolesAsync(IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.PermissionId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> NameExistsInScopeAsync(string name, string? scope, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles
                .AnyAsync(r =>
                    r.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    (r.Scope == scope || r.Scope == null),
                    cancellationToken);
        }

        public async Task RemovePermissionFromRoleAsync(Guid roleId, string permissionId, CancellationToken cancellationToken = default)
        {
            var link = await _dbContext.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);

            if (link is not null)
            {
                _dbContext.RolePermissions.Remove(link);
            }
        }

        public async Task<List<Guid>> GetRolesContainingPermissionAsync(string permissionId, string scope, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles
                .Where(r => r.Scope == scope || r.Scope == null)
                .Join(_dbContext.RolePermissions.Where(rp => rp.PermissionId == permissionId),
                    role => role.Id,
                    rolePermission => rolePermission.RoleId,
                    (role, rolePermission) => role.Id)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Guid>> GetRolesByPermissionIdAsync(string permissionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .Select(rp => rp.RoleId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Role>> GetRolesByBaseRoleIdAsync(Guid baseRoleId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles
                .Where(r => r.BaseRoleId == baseRoleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Name == name && r.Scope == null, cancellationToken);
        }
    }
}
