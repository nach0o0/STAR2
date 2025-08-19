using Planning.Application.Common.Dtos;
using Planning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Interfaces.Persistence
{
    public interface IPlanningEntryRepository
    {
        Task<PlanningEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(PlanningEntry planningEntry, CancellationToken cancellationToken = default);
        void Delete(PlanningEntry planningEntry);
        Task DeleteByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task DeleteByCostObjectIdAsync(Guid costObjectId, CancellationToken cancellationToken = default);
        Task<List<PlanningEntry>> GetByGroupIdAndDateRangeAsync(
            Guid employeeGroupId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
        Task<List<PlanningEntry>> GetByEmployeeIdAndGroupIdAndDateRangeAsync(
            Guid employeeId,
            Guid employeeGroupId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
        Task<List<PlanningEntry>> GetByCostObjectIdAndDateRangeAsync(
            Guid costObjectId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
        Task<List<PlanningSummaryDto>> GetSummaryByCostObjectAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<List<PlanningSummaryDto>> GetSummaryByEmployeeAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
