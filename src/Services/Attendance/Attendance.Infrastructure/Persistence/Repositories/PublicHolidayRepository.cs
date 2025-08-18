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
    public class PublicHolidayRepository : IPublicHolidayRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public PublicHolidayRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PublicHoliday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PublicHolidays.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(PublicHoliday holiday, CancellationToken cancellationToken = default)
        {
            await _dbContext.PublicHolidays.AddAsync(holiday, cancellationToken);
        }

        public void Delete(PublicHoliday holiday)
        {
            _dbContext.PublicHolidays.Remove(holiday);
        }

        public async Task<List<PublicHoliday>> GetByEmployeeGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PublicHolidays
                .Where(ph => ph.EmployeeGroupId == employeeGroupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<PublicHoliday>> GetByDateRangeAndGroupsAsync(IEnumerable<Guid> employeeGroupIds, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PublicHolidays
                .Where(ph => employeeGroupIds.Contains(ph.EmployeeGroupId) &&
                             ph.Date >= startDate.Date &&
                             ph.Date <= endDate.Date)
                .OrderBy(ph => ph.Date)
                .ToListAsync(cancellationToken);
        }
    }
}
