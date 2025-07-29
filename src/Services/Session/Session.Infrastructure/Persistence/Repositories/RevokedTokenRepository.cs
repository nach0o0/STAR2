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
    public class RevokedTokenRepository : IRevokedTokenRepository
    {
        private readonly SessionDbContext _dbContext;

        public RevokedTokenRepository(SessionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Guid jti, DateTime expiresAt, CancellationToken cancellationToken = default)
        {
            var revokedToken = new RevokedToken(jti, expiresAt);
            await _dbContext.RevokedTokens.AddAsync(revokedToken, cancellationToken);
        }

        public async Task<bool> IsTokenRevokedAsync(Guid jti, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RevokedTokens.AnyAsync(t => t.Jti == jti, cancellationToken);
        }
    }
}
