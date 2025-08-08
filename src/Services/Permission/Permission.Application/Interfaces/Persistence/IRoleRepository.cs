using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Interfaces.Persistence
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<string>> GetPermissionsForRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
        Task<List<string>> GetPermissionsForRolesAsync(IEnumerable<Guid> roleIds, CancellationToken cancellationToken = default);
        Task<List<Guid>> GetRolesContainingPermissionAsync(string permissionId, string scope, CancellationToken cancellationToken = default);
        Task<bool> NameExistsInScopeAsync(string name, string? scope, CancellationToken cancellationToken = default);
        Task AddAsync(Role role, CancellationToken cancellationToken = default);
        void Delete(Role role);
        Task RemovePermissionFromRoleAsync(Guid roleId, string permissionId, CancellationToken cancellationToken = default);
        Task<List<Guid>> GetRolesByPermissionIdAsync(string permissionId, CancellationToken cancellationToken = default);
        Task<List<Role>> GetRolesByBaseRoleIdAsync(Guid baseRoleId, CancellationToken cancellationToken = default);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default); 
        Task<List<Role>> GetByScopeAsync(string scope, CancellationToken cancellationToken = default);
    }
}
