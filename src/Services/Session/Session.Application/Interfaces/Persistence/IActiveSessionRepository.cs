using Session.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Interfaces.Persistence
{
    public interface IActiveSessionRepository
    {
        Task<ActiveSession?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken = default);

        Task AddAsync(ActiveSession session, CancellationToken cancellationToken = default);
    }
}
