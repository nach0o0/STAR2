using Microsoft.EntityFrameworkCore;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Persistence.Repositories
{
    public class HourlyRateRepository : IHourlyRateRepository
    {
        private readonly OrganizationDbContext _dbContext;

        public HourlyRateRepository(OrganizationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HourlyRate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.HourlyRates.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<HourlyRate>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.HourlyRates
                .Where(hr => hr.OrganizationId == organizationId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(HourlyRate hourlyRate, CancellationToken cancellationToken = default)
        {
            await _dbContext.HourlyRates.AddAsync(hourlyRate, cancellationToken);
        }

        public void Delete(HourlyRate hourlyRate)
        {
            _dbContext.HourlyRates.Remove(hourlyRate);
        }
    }
}
