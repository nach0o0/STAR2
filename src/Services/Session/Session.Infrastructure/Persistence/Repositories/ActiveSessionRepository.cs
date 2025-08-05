using Microsoft.EntityFrameworkCore;
using Session.Application.Interfaces.Persistence;
using Session.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Infrastructure.Persistence.Repositories
{
    public class ActiveSessionRepository : IActiveSessionRepository
    {
        private readonly SessionDbContext _dbContext;

        public ActiveSessionRepository(SessionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActiveSession?> GetBySelectorAsync(string selector, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ActiveSessions
                .FirstOrDefaultAsync(s => s.Selector == selector, cancellationToken);
        }

        public async Task AddAsync(ActiveSession session, CancellationToken cancellationToken = default)
        {
            await _dbContext.ActiveSessions.AddAsync(session, cancellationToken);
        }

        public void Delete(ActiveSession session)
        {
            _dbContext.ActiveSessions.Remove(session);
        }

        public async Task<List<ActiveSession>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ActiveSessions
                .Where(s => s.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
