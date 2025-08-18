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
    public class EmployeeWorkModelRepository : IEmployeeWorkModelRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public EmployeeWorkModelRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeWorkModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeWorkModels.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(EmployeeWorkModel assignment, CancellationToken cancellationToken = default)
        {
            await _dbContext.EmployeeWorkModels.AddAsync(assignment, cancellationToken);
        }

        public void Delete(EmployeeWorkModel assignment)
        {
            _dbContext.EmployeeWorkModels.Remove(assignment);
        }

        public async Task<List<EmployeeWorkModel>> GetByWorkModelIdAsync(Guid workModelId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeWorkModels
                .Where(ewm => ewm.WorkModelId == workModelId)
                .ToListAsync(cancellationToken);
        }

        public async Task<EmployeeWorkModel?> GetActiveAssignmentForEmployeeAsync(Guid employeeId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeWorkModels
                .Where(ewm => ewm.EmployeeId == employeeId &&
                              ewm.ValidFrom <= date.Date &&
                              (ewm.ValidTo == null || ewm.ValidTo >= date.Date))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<EmployeeWorkModel>> GetAssignmentsForEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeWorkModels
                .Where(ewm => ewm.EmployeeId == employeeId)
                .OrderBy(ewm => ewm.ValidFrom)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EmployeeWorkModel>> GetActiveAssignmentsForEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeWorkModels
                .Where(ewm => ewm.EmployeeId == employeeId && ewm.ValidTo == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EmployeeWorkModel>> GetAssignmentsByWorkModelIdsAsync(IEnumerable<Guid> workModelIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeWorkModels
                .Where(ewm => workModelIds.Contains(ewm.WorkModelId))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<EmployeeWorkModel>> GetAssignmentsForEmployeeByDateRangeAsync(Guid employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmployeeWorkModels
                .Where(ewm => ewm.EmployeeId == employeeId &&
                              ewm.ValidFrom <= endDate.Date &&
                              (ewm.ValidTo == null || ewm.ValidTo >= startDate.Date))
                .OrderBy(ewm => ewm.ValidFrom)
                .ToListAsync(cancellationToken);
        }
    }
}
