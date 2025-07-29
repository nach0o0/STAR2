using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Interfaces.Persistence
{
    public interface IRoleRepository
    {
        Task<List<string>> GetPermissionsForRolesAsync(
            IEnumerable<Guid> roleIds,
            CancellationToken cancellationToken = default);
    }
}
