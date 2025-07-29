using Microsoft.EntityFrameworkCore;
using Permission.Application.Interfaces.Persistence;
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

        public async Task<List<string>> GetPermissionsForRolesAsync(
            IEnumerable<Guid> roleIds,
            CancellationToken cancellationToken = default)
        {
            // Findet alle PermissionIds, die mit den übergebenen RoleIds verknüpft sind.
            return await _dbContext.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.PermissionId)
                .Distinct()
                .ToListAsync(cancellationToken);
        }
    }
}
