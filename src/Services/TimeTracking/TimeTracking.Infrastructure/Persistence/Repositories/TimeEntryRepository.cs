using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Common.Dtos;
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

        public async Task DeleteByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            // Führt eine effiziente Massenlöschung direkt auf der Datenbank aus.
            await _dbContext.TimeEntries
                .Where(te => te.EmployeeGroupId == employeeGroupId)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task BulkUpdateCostObjectIdAsync(Guid oldCostObjectId, Guid? newCostObjectId, CancellationToken cancellationToken = default)
        {
            await _dbContext.TimeEntries
                .Where(te => te.CostObjectId == oldCostObjectId)
                .ExecuteUpdateAsync(updates => updates.SetProperty(te => te.CostObjectId, newCostObjectId), cancellationToken);
        }

        public async Task<List<TimeEntry>> GetPersonalAndAnonymizedEntriesAsync(Guid employeeId, DateTime startDate, DateTime endDate, List<Guid>? accessKeys, CancellationToken cancellationToken = default)
        {
            // Konvertiere die Daten in UTC für einen sauberen DB-Vergleich
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.TimeEntries
                .Where(te => te.EntryDate >= start && te.EntryDate <= end)
                .Where(te =>
                    // Bedingung 1: Der Eintrag gehört dem Benutzer
                    te.EmployeeId == employeeId ||
                    // ODER Bedingung 2: Der Eintrag ist anonym und der Schlüssel wurde mitgeliefert
                    (te.IsAnonymized && accessKeys != null && te.AccessKey.HasValue && accessKeys.Contains(te.AccessKey.Value))
                )
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TimeEntry>> GetByGroupAndDateRangeAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.TimeEntries
                .Where(te =>
                    te.EmployeeGroupId == employeeGroupId &&
                    !te.IsAnonymized && // Nur nicht-anonymisierte Einträge
                    te.EntryDate >= start &&
                    te.EntryDate <= end)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TimeEntry>> GetByCostObjectAndDateRangeAsync(Guid costObjectId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.TimeEntries
                .Where(te =>
                    te.CostObjectId == costObjectId &&
                    !te.IsAnonymized &&
                    te.EntryDate >= start &&
                    te.EntryDate <= end)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TimeSummaryDto>> GetSummaryByCostObjectAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.TimeEntries
                .Where(te =>
                    te.EmployeeGroupId == employeeGroupId &&
                    !te.IsAnonymized &&
                    te.CostObjectId.HasValue &&
                    te.EntryDate >= start &&
                    te.EntryDate <= end)
                .GroupBy(te => te.CostObjectId)
                .Select(g => new TimeSummaryDto(
                    g.Key!.Value,
                    "", // Der Name wird später vom CostObjectService geholt
                    g.Sum(te => te.Hours)
                ))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TimeSummaryDto>> GetSummaryByEmployeeAsync(Guid employeeGroupId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var start = startDate.Date.ToUniversalTime();
            var end = endDate.Date.ToUniversalTime();

            return await _dbContext.TimeEntries
                .Where(te =>
                    te.EmployeeGroupId == employeeGroupId &&
                    !te.IsAnonymized &&
                    te.EmployeeId.HasValue && // Nur Einträge mit Mitarbeiterbezug
                    te.EntryDate >= start &&
                    te.EntryDate <= end)
                .GroupBy(te => te.EmployeeId)
                .Select(g => new TimeSummaryDto(
                    g.Key!.Value,
                    "", // Der Name wird später vom OrganizationService geholt
                    g.Sum(te => te.Hours)
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
