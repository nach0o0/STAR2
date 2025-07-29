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
    }
}
