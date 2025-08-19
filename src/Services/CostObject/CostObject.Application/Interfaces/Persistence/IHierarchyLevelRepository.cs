using CostObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Interfaces.Persistence
{
    public interface IHierarchyLevelRepository
    {
        Task<HierarchyLevel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(HierarchyLevel hierarchyLevel, CancellationToken cancellationToken = default);
        void Delete(HierarchyLevel hierarchyLevel);
        Task<bool> DepthExistsInHierarchyAsync(int depth, Guid hierarchyDefinitionId, CancellationToken cancellationToken = default);
        Task<bool> IsHierarchyDefinitionInUseAsync(Guid hierarchyDefinitionId, CancellationToken cancellationToken = default);
        Task<List<HierarchyLevel>> GetByHierarchyDefinitionIdAsync(Guid hierarchyDefinitionId, CancellationToken cancellationToken = default);
        Task<List<HierarchyLevel>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
