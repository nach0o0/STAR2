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
    }
}
