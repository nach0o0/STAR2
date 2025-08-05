using Microsoft.EntityFrameworkCore;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly PermissionDbContext _dbContext;

        public PermissionRepository(PermissionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.Permission?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Permissions.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<Domain.Entities.Permission> permissions, CancellationToken cancellationToken = default)
        {
            // Fügt nur Berechtigungen hinzu, die noch nicht existieren.
            var existingPermissionIds = await _dbContext.Permissions
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            var newPermissions = permissions.Where(p => !existingPermissionIds.Contains(p.Id));

            if (newPermissions.Any())
            {
                await _dbContext.Permissions.AddRangeAsync(newPermissions, cancellationToken);
            }
        }
    }
}
