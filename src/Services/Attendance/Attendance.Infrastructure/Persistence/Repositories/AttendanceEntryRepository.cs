using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Persistence.Repositories
{
    public class AttendanceEntryRepository : IAttendanceEntryRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public AttendanceEntryRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AttendanceEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceEntries.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(AttendanceEntry entry, CancellationToken cancellationToken = default)
        {
            await _dbContext.AttendanceEntries.AddAsync(entry, cancellationToken);
        }

        public void Delete(AttendanceEntry entry)
        {
            _dbContext.AttendanceEntries.Remove(entry);
        }

        public async Task<bool> IsAttendanceTypeInUseAsync(Guid attendanceTypeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceEntries
                .AnyAsync(e => e.AttendanceTypeId == attendanceTypeId, cancellationToken);
        }

        public async Task<AttendanceEntry?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceEntries
                .Include(e => e.AttendanceType)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId && e.Date == date.Date, cancellationToken);
        }

        public async Task<List<AttendanceEntry>> GetForEmployeeInGroupsByDateRangeAsync(Guid employeeId, IEnumerable<Guid> employeeGroupIds, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceEntries
                .Include(e => e.AttendanceType) // JOIN mit AttendanceType
                .Where(e => e.EmployeeId == employeeId &&
                             e.Date >= startDate.Date &&
                             e.Date <= endDate.Date &&
                             employeeGroupIds.Contains(e.AttendanceType.EmployeeGroupId))
                .OrderBy(e => e.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AttendanceEntry>> GetForEmployeesByDateRangeAsync(IEnumerable<Guid> employeeIds, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceEntries
                .Include(e => e.AttendanceType)
                .Where(e => employeeIds.Contains(e.EmployeeId) &&
                             e.Date >= startDate.Date &&
                             e.Date <= endDate.Date)
                .OrderBy(e => e.Date)
                .ThenBy(e => e.EmployeeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<AttendanceType, int>> GetSummaryByEmployeeAndDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceEntries
                .Where(e => e.EmployeeId == employeeId &&
                             e.Date >= startDate.Date &&
                             e.Date <= endDate.Date)
                .Include(e => e.AttendanceType)
                .GroupBy(e => e.AttendanceType)
                .Select(g => new { AttendanceType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.AttendanceType, x => x.Count, cancellationToken);
        }

        public async Task<List<AttendanceEntry>> GetAbsencesForEmployeesByDateAsync(IEnumerable<Guid> employeeIds, DateTime date, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceEntries
                .Include(e => e.AttendanceType)
                .Where(e => employeeIds.Contains(e.EmployeeId) &&
                             e.Date == date.Date &&
                             e.AttendanceType.IsPresent == false)
                .ToListAsync(cancellationToken);
        }
    }
}
