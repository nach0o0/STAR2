using Microsoft.EntityFrameworkCore;
using Planning.Application.Common.Dtos;
using Planning.Application.Interfaces.Persistence;
using Planning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Infrastructure.Persistence.Repositories
{
    public class PlanningEntryRepository : IPlanningEntryRepository
    {
        private readonly PlanningDbContext _dbContext;

        public PlanningEntryRepository(PlanningDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlanningEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PlanningEntries.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(PlanningEntry planningEntry, CancellationToken cancellationToken = default)
        {
            await _dbContext.PlanningEntries.AddAsync(planningEntry, cancellationToken);
        }

        public void Delete(PlanningEntry planningEntry)
        {
            _dbContext.PlanningEntries.Remove(planningEntry);
        }

        public async Task DeleteByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            await _dbContext.PlanningEntries
                .Where(p => p.EmployeeGroupId == employeeGroupId)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task DeleteByCostObjectIdAsync(Guid costObjectId, CancellationToken cancellationToken = default)
        {
            await _dbContext.PlanningEntries
                .Where(p => p.CostObjectId == costObjectId)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<List<PlanningEntry>> GetByGroupIdAndDateRangeAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.PlanningEntries
                .Where(p =>
                    p.EmployeeGroupId == employeeGroupId &&
                    p.PlanningPeriodStart <= end &&
                    p.PlanningPeriodEnd >= start)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PlanningEntry>> GetByEmployeeIdAndGroupIdAndDateRangeAsync(Guid employeeId, Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.PlanningEntries
                .Where(p =>
                    p.EmployeeId == employeeId &&
                    p.EmployeeGroupId == employeeGroupId &&
                    p.PlanningPeriodStart <= end &&
                    p.PlanningPeriodEnd >= start)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PlanningEntry>> GetByCostObjectIdAndDateRangeAsync(Guid costObjectId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.PlanningEntries
                .Where(p =>
                    p.CostObjectId == costObjectId &&
                    p.PlanningPeriodStart <= end &&
                    p.PlanningPeriodEnd >= start)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PlanningSummaryDto>> GetSummaryByCostObjectAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.PlanningEntries
                .Where(p => p.EmployeeGroupId == employeeGroupId && p.PlanningPeriodStart <= end && p.PlanningPeriodEnd >= start)
                .GroupBy(p => p.CostObjectId)
                .Select(g => new PlanningSummaryDto(g.Key, g.Sum(p => p.PlannedHours)))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PlanningSummaryDto>> GetSummaryByEmployeeAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.PlanningEntries
                .Where(p => p.EmployeeGroupId == employeeGroupId && p.PlanningPeriodStart <= end && p.PlanningPeriodEnd >= start)
                .GroupBy(p => p.EmployeeId)
                .Select(g => new PlanningSummaryDto(g.Key, g.Sum(p => p.PlannedHours)))
                .ToListAsync(cancellationToken);
        }
    }
}
