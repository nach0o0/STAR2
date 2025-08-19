using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Domain.Entities;

namespace TimeTracking.Application.Interfaces.Persistence
{
    public interface ITimeEntryRepository
    {
        Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TimeEntry?> GetByAccessKeyAsync(Guid accessKey, CancellationToken cancellationToken = default);
        Task AddAsync(TimeEntry timeEntry, CancellationToken cancellationToken = default);
        void Delete(TimeEntry timeEntry);
    }
}
