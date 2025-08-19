using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Common.Dtos;
using TimeTracking.Domain.Entities;

namespace TimeTracking.Application.Interfaces.Persistence
{
    public interface ITimeEntryRepository
    {
        Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TimeEntry?> GetByAccessKeyAsync(Guid accessKey, CancellationToken cancellationToken = default);
        Task AddAsync(TimeEntry timeEntry, CancellationToken cancellationToken = default);
        void Delete(TimeEntry timeEntry);
        Task DeleteByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task BulkUpdateCostObjectIdAsync(Guid oldCostObjectId, Guid? newCostObjectId, CancellationToken cancellationToken = default);
        Task<List<TimeEntry>> GetPersonalAndAnonymizedEntriesAsync(
            Guid employeeId,
            DateTime startDate,
            DateTime endDate,
            List<Guid>? accessKeys,
            CancellationToken cancellationToken = default);
        Task<List<TimeEntry>> GetByGroupAndDateRangeAsync(
            Guid employeeGroupId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
        Task<List<TimeEntry>> GetByCostObjectAndDateRangeAsync(
            Guid costObjectId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
        Task<List<TimeSummaryDto>> GetSummaryByCostObjectAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<List<TimeSummaryDto>> GetSummaryByEmployeeAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
