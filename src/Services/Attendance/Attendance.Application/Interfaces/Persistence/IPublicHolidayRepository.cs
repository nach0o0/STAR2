using Attendance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interfaces.Persistence
{
    public interface IPublicHolidayRepository
    {
        Task<PublicHoliday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(PublicHoliday holiday, CancellationToken cancellationToken = default);
        void Delete(PublicHoliday holiday);
        Task<List<PublicHoliday>> GetByEmployeeGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<PublicHoliday>> GetByDateRangeAndGroupsAsync(IEnumerable<Guid> employeeGroupIds, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
