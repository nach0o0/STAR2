using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Infrastructure.Persistence.Repositories
{
    public class HierarchyLevelRepository : IHierarchyLevelRepository
    {
        private readonly CostObjectDbContext _dbContext;

        public HierarchyLevelRepository(CostObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HierarchyLevel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.HierarchyLevels.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(HierarchyLevel hierarchyLevel, CancellationToken cancellationToken = default)
        {
            await _dbContext.HierarchyLevels.AddAsync(hierarchyLevel, cancellationToken);
        }

        public void Delete(HierarchyLevel hierarchyLevel)
        {
            _dbContext.HierarchyLevels.Remove(hierarchyLevel);
        }
    }
}
