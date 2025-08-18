using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
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
    }
}
