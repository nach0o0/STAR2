using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Interfaces.Persistence
{
    public interface ICostObjectRepository
    {
        Task<Domain.Entities.CostObject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Domain.Entities.CostObject costObject, CancellationToken cancellationToken = default);
        void Delete(Domain.Entities.CostObject costObject);
        Task<bool> IsHierarchyLevelInUseAsync(Guid hierarchyLevelId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetTopLevelCostObjectsByGroupAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetAllDescendantsAsync(Guid parentCostObjectId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetByLabelIdAsync(Guid labelId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetForGroupWithRequestsAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetActiveAndApprovedByGroupAsync(Guid employeeGroupId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetChildrenAsync(Guid parentCostObjectId, CancellationToken cancellationToken = default);
        Task<bool> HasChildrenAsync(Guid costObjectId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.CostObject>> GetTopLevelByGroupAsync(Guid employeeGroupId, bool approvedOnly, bool activeOnly, CancellationToken cancellationToken = default);
    }
}
