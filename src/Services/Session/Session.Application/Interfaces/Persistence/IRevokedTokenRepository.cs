using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Interfaces.Persistence
{
    public interface IRevokedTokenRepository
    {
        Task AddAsync(Guid jti, DateTime expiresAt, CancellationToken cancellationToken = default);

        Task<bool> IsTokenRevokedAsync(Guid jti, CancellationToken cancellationToken = default);
    }
}
