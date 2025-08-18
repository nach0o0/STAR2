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
    public class AttendanceTypeRepository : IAttendanceTypeRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public AttendanceTypeRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AttendanceType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceTypes.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(AttendanceType type, CancellationToken cancellationToken = default)
        {
            await _dbContext.AttendanceTypes.AddAsync(type, cancellationToken);
        }

        public void Delete(AttendanceType type)
        {
            _dbContext.AttendanceTypes.Remove(type);
        }

        public async Task<List<AttendanceType>> GetByEmployeeGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.AttendanceTypes
                .Where(at => at.EmployeeGroupId == employeeGroupId)
                .ToListAsync(cancellationToken);
        }
    }
}
