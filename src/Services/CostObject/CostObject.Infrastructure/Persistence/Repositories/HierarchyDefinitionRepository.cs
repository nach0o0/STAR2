using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Infrastructure.Persistence.Repositories
{
    public class HierarchyDefinitionRepository : IHierarchyDefinitionRepository
    {
        private readonly CostObjectDbContext _dbContext;

        public HierarchyDefinitionRepository(CostObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HierarchyDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.HierarchyDefinitions.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddAsync(HierarchyDefinition hierarchyDefinition, CancellationToken cancellationToken = default)
        {
            await _dbContext.HierarchyDefinitions.AddAsync(hierarchyDefinition, cancellationToken);
        }

        public void Delete(HierarchyDefinition hierarchyDefinition)
        {
            _dbContext.HierarchyDefinitions.Remove(hierarchyDefinition);
        }
    }
}
