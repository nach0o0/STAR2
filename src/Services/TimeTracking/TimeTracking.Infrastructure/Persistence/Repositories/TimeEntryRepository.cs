using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;
using TimeTracking.Domain.Entities;

namespace TimeTracking.Infrastructure.Persistence.Repositories
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly TimeTrackingDbContext _dbContext;

        public TimeEntryRepository(TimeTrackingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TimeEntries.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<TimeEntry?> GetByAccessKeyAsync(Guid accessKey, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TimeEntries
                .FirstOrDefaultAsync(te => te.AccessKey == accessKey, cancellationToken);
        }

        public async Task AddAsync(TimeEntry timeEntry, CancellationToken cancellationToken = default)
        {
            await _dbContext.TimeEntries.AddAsync(timeEntry, cancellationToken);
        }

        public void Delete(TimeEntry timeEntry)
        {
            _dbContext.TimeEntries.Remove(timeEntry);
        }
    }
}
