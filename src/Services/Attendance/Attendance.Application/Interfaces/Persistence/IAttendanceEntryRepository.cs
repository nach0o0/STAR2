using Attendance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interfaces.Persistence
{
    public interface IAttendanceEntryRepository
    {
        Task<AttendanceEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(AttendanceEntry entry, CancellationToken cancellationToken = default);
        void Delete(AttendanceEntry entry);
        Task<bool> IsAttendanceTypeInUseAsync(Guid attendanceTypeId, CancellationToken cancellationToken = default);
        Task<AttendanceEntry?> GetByEmployeeAndDateAsync(Guid employeeId, DateTime date, CancellationToken cancellationToken = default);
        Task<List<AttendanceEntry>> GetForEmployeeInGroupsByDateRangeAsync(Guid employeeId, IEnumerable<Guid> employeeGroupIds, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<List<AttendanceEntry>> GetForEmployeesByDateRangeAsync(IEnumerable<Guid> employeeIds, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<Dictionary<AttendanceType, int>> GetSummaryByEmployeeAndDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<List<AttendanceEntry>> GetAbsencesForEmployeesByDateAsync(IEnumerable<Guid> employeeIds, DateTime date, CancellationToken cancellationToken = default);
    }
}
