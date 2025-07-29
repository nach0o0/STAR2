using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Interfaces.Persistence
{
    public interface IPermissionRepository
    {
        Task AddRangeAsync(IEnumerable<Domain.Entities.Permission> permissions, CancellationToken cancellationToken = default);
    }
}
