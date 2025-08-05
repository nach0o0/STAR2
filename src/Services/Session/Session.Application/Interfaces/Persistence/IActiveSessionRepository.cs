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
        Task<ActiveSession?> GetBySelectorAsync(string selector, CancellationToken cancellationToken = default);
        Task AddAsync(ActiveSession session, CancellationToken cancellationToken = default);
        void Delete(ActiveSession session);
        Task<List<ActiveSession>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
