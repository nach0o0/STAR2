using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Enums;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> IsHierarchyLevelInUseAsync(Guid hierarchyLevelId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects.AnyAsync(co => co.HierarchyLevelId == hierarchyLevelId, cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetTopLevelCostObjectsByGroupAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects
                .Where(co => co.EmployeeGroupId == employeeGroupId && co.ParentCostObjectId == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetAllDescendantsAsync(Guid parentCostObjectId, CancellationToken cancellationToken = default)
        {
            var allCostObjects = await _dbContext.CostObjects.ToListAsync(cancellationToken);
            var descendants = new List<Domain.Entities.CostObject>();
            var children = allCostObjects.Where(co => co.ParentCostObjectId == parentCostObjectId).ToList();

            while (children.Any())
            {
                descendants.AddRange(children);
                var childrenIds = children.Select(c => c.Id).ToList();
                children = allCostObjects.Where(co => co.ParentCostObjectId.HasValue && childrenIds.Contains(co.ParentCostObjectId.Value)).ToList();
            }
            return descendants;
        }

        public async Task<List<Domain.Entities.CostObject>> GetByLabelIdAsync(Guid labelId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects
                .Where(co => co.LabelId == labelId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetByGroupIdAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects
                .Where(co => co.EmployeeGroupId == employeeGroupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetForGroupWithRequestsAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects
                .Where(co => co.EmployeeGroupId == employeeGroupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects
                .Where(co => ids.Contains(co.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetChildrenAsync(Guid parentCostObjectId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects
                .Where(co => co.ParentCostObjectId == parentCostObjectId)
                .OrderBy(co => co.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasChildrenAsync(Guid costObjectId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.CostObjects.AnyAsync(co => co.ParentCostObjectId == costObjectId, cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetTopLevelByGroupAsync(Guid employeeGroupId, bool approvedOnly, bool activeOnly, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.CostObjects
                .Where(co => co.EmployeeGroupId == employeeGroupId && co.ParentCostObjectId == null);

            if (approvedOnly)
            {
                query = query.Where(co => co.ApprovalStatus == ApprovalStatus.Approved);
            }

            if (activeOnly)
            {
                var today = DateTime.UtcNow.Date;
                query = query.Where(co => co.ValidFrom.Date <= today && (co.ValidTo == null || co.ValidTo.Value.Date >= today));
            }

            return await query.OrderBy(co => co.Name).ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.CostObject>> GetActiveAndApprovedByGroupAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            return await _dbContext.CostObjects
                .Where(co =>
                    co.EmployeeGroupId == employeeGroupId &&
                    co.ApprovalStatus == ApprovalStatus.Approved &&
                    co.ValidFrom.Date <= today &&
                    (co.ValidTo == null || co.ValidTo.Value.Date >= today))
                .ToListAsync(cancellationToken);
        }
    }
}
