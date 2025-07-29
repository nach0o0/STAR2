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

        public UserPermissionAssignmentRepository(PermissionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserPermissionAssignment>> GetAssignmentsForUserAsync(Guid userId, IEnumerable<string> scopes, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserPermissionAssignments
                .Where(a => a.UserId == userId && scopes.Contains(a.Scope))
                .ToListAsync(cancellationToken);
        }
    }
}
