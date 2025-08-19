using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DepthExistsInHierarchyAsync(int depth, Guid hierarchyDefinitionId, CancellationToken cancellationToken = default)
        {
            // Prüft in der Tabelle "HierarchyLevels", ob es bereits einen Eintrag gibt,
            // der dieselbe HierarchyDefinitionId UND dieselbe Tiefe (Depth) hat.
            return await _dbContext.HierarchyLevels
                .AnyAsync(level => level.HierarchyDefinitionId == hierarchyDefinitionId && level.Depth == depth, cancellationToken);
        }

        public async Task<bool> IsHierarchyDefinitionInUseAsync(Guid hierarchyDefinitionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.HierarchyLevels.AnyAsync(level => level.HierarchyDefinitionId == hierarchyDefinitionId, cancellationToken);
        }

        public async Task<List<HierarchyLevel>> GetByHierarchyDefinitionIdAsync(Guid hierarchyDefinitionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.HierarchyLevels
                .Where(level => level.HierarchyDefinitionId == hierarchyDefinitionId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<HierarchyLevel>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _dbContext.HierarchyLevels
                .Where(l => ids.Contains(l.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
