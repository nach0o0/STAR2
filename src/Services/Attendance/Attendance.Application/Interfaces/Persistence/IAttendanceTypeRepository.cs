using Attendance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interfaces.Persistence
{
    public interface IAttendanceTypeRepository
    {
        Task<AttendanceType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(AttendanceType type, CancellationToken cancellationToken = default);
        void Delete(AttendanceType type);
        Task<List<AttendanceType>> GetByEmployeeGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
    }
}
