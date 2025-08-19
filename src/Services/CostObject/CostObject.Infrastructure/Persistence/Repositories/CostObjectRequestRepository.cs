using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using CostObject.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Infrastructure.Persistence.Repositories
{
    public class CostObjectRequestRepository : ICostObjectRequestRepository
    {
        private readonly CostObjectDbContext _dbContext;

        public CostObjectRequestRepository(CostObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CostObjectRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjectRequests.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(CostObjectRequest costObjectRequest, CancellationToken cancellationToken = default)
        {
            await _dbContext.CostObjectRequests.AddAsync(costObjectRequest, cancellationToken);
        }

        public void Delete(CostObjectRequest costObjectRequest)
        {
            _dbContext.CostObjectRequests.Remove(costObjectRequest);
        }

        public async Task<List<CostObjectRequest>> GetByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjectRequests
                .Where(co => co.EmployeeGroupId == employeeGroupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CostObjectRequest>> GetPendingByRequesterAsync(Guid requesterEmployeeId, Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjectRequests
                .Where(r => r.EmployeeGroupId == employeeGroupId &&
                            r.RequesterEmployeeId == requesterEmployeeId &&
                            r.Status == ApprovalStatus.Pending)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CostObjectRequest>> GetByRequesterAndGroupAsync(Guid requesterEmployeeId, Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjectRequests
                .Where(r => r.RequesterEmployeeId == requesterEmployeeId && r.EmployeeGroupId == employeeGroupId)
                .ToListAsync(cancellationToken);
        }
    }
}
