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
    public class WorkModelRepository : IWorkModelRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public WorkModelRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.WorkModels.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(WorkModel model, CancellationToken cancellationToken = default)
        {
            await _dbContext.WorkModels.AddAsync(model, cancellationToken);
        }

        public void Delete(WorkModel model)
        {
            _dbContext.WorkModels.Remove(model);
        }

        public async Task<List<WorkModel>> GetByEmployeeGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.WorkModels
                .Where(wm => wm.EmployeeGroupId == employeeGroupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<WorkModel>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _dbContext.WorkModels
                .Where(wm => ids.Contains(wm.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
