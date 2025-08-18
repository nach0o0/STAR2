using CostObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Interfaces.Persistence
{
    public interface IHierarchyDefinitionRepository
    {
        Task<HierarchyDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(HierarchyDefinition hierarchyDefinition, CancellationToken cancellationToken = default);
        void Delete(HierarchyDefinition hierarchyDefinition);
    }
}
