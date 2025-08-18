using CostObject.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Infrastructure.Persistence.Repositories
{
    public class CostObjectRepository : ICostObjectRepository
    {
        private readonly CostObjectDbContext _dbContext;

        public CostObjectRepository(CostObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.CostObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(Domain.Entities.CostObject costObject, CancellationToken cancellationToken = default)
        {
            await _dbContext.CostObjects.AddAsync(costObject, cancellationToken);
        }

        public void Delete(Domain.Entities.CostObject costObject)
        {
            _dbContext.CostObjects.Remove(costObject);
        }
    }
}
